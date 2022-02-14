using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ASP.NET_DnD_App.Models;
using ASP.NET_DnD_App.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace ASP.NET_DnD_App.Controllers
{
    public class CampaignController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly IEmailProvider _emailProvider;


        public CampaignController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IEmailProvider emailProvider)
        {
            _context = context;
            _userManager = userManager;
            _emailProvider = emailProvider;
        }

        public IActionResult Index() // View all players in the current DungeonMatser's campaign
        {
            return View();
        }

        [HttpGet]
        public IActionResult DeletePlayer() // Delete BasicPlayer from campaign
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePlayerConfirmed()
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Invite()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Invite(CampaignInvites invite)
        {
            if (ModelState.IsValid)
            {
                // Get BasicPlayer's user name
                string? PlayersUserName = invite.InvitedPlayerUserName;

                // Get the targeted user
                IdentityUser basicPlayer = await _userManager.FindByNameAsync(PlayersUserName);

                if (basicPlayer == null)
                {
                    ViewData["FindUser"] = $"{invite.InvitedPlayerUserName} could not be found. Check your spelling and try again.";

                    // View the from
                    return View();
                }

                // Make sure the targeted user is a BasicPlayer
                bool isBasicPlayer = await _userManager.IsInRoleAsync(basicPlayer, "Basic Player");

                if(!isBasicPlayer) // If player is not a BasicPlayer return the form
                {
                    ViewData["IsBasic"] = $"{invite.InvitedPlayerUserName} is not a Basic Player. You cannot invite other Dungeon Masters to your campaign.";

                    return View();
                }

                // Get current user (DungeonMaster)
                IdentityUser currentUser = await _userManager.GetUserAsync(User);

                invite.DungeonMaster = currentUser;

                // Get Invite code
                Random rand = new Random();

                int inviteCode = rand.Next(100000, 999999 + 1); // Only get a 6 digit code

                // Assign the invite code to the invite
                invite.InviteCode = inviteCode;

                // Assign DungeonMaster to the invite
                invite.DungeonMaster = currentUser;

                // Check if BasicPlayer is already part of a campaign
                bool inCampaign = await CampaignInvitesDB.IsInCampaign(_context, basicPlayer);

                if (inCampaign)
                {
                    ViewData["IsInCampaign"] = $"{invite.InvitedPlayerUserName} is already in a campaign.";

                    return View();
                }

                // Check if basic player has a invite pending
                int inviteNumber = await CampaignInvitesDB.HasInvite(_context, basicPlayer);

                if (inviteNumber > 0) // If the number of invites is larger than one, don't allow to send another invite. (invite number cannot be negative)
                {
                    ViewData["InvitePlayer"] = $"{invite.InvitedPlayerUserName} already has a pending invite to a campaign.";
                    return View();
                }

                // If we got this far we can start the invite process
                // Send the targeted BasicPlayer the invite and invite code
                string toEmail = basicPlayer.Email;
                string fromEmail = "dndmanager.noreply@gmail.com";
                string subject = "Campaign Invite";
                string body = currentUser.UserName + " has invited you to a campaign! \n\n Your invite code is: " + invite.InviteCode;
                string htmlContent = "";

                var response = await _emailProvider.SendEmailAsync(basicPlayer.UserName, toEmail, fromEmail, subject, body, htmlContent);

                if(response.IsSuccessStatusCode)
                {
                    // If the email was successfully sent, add the invite to the database
                    await CampaignInvitesDB.SendInvite(_context, invite);
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult AcceptInvite()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AcceptInvite(InviteCode inviteCode) 
        {
            return View();
        }
    }
}
