﻿using ASP.NET_DnD_App.Data;
using ASP.NET_DnD_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_DnD_App.Controllers
{
    public class InviteController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;

        private readonly IEmailProvider _emailProvider;

        public InviteController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IEmailProvider emailProvider)
        {
            _context = context;
            _userManager = userManager;
            _emailProvider = emailProvider;
        }

        [Authorize(Roles = "Dungeon Master")]
        public async Task<IActionResult> IndexAsync()
        {
            // Set ViewData["InviteIndex"] != null
            ViewData["InviteIndex"] = "Not null";

            // Get the current user
            IdentityUser currentUser = await _userManager.GetUserAsync(User);

            // Get all invites that were sent from the current DungeonMaster
            List<CampaignInvites> sentInvites = await CampaignInvitesDB.GetCampaignInvitesAsync(_context, currentUser);

            // Return all the invite to the view
            return View(sentInvites);
        }

        [HttpGet]
        [Authorize(Roles = "Dungeon Master")]
        public IActionResult Invite()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Dungeon Master")]
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

                if (!isBasicPlayer) // If player is not a BasicPlayer return the form
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
                bool inCampaign = await CampaignDB.IsInCampaignAsync(_context, basicPlayer);

                if (inCampaign)
                {
                    ViewData["IsInCampaign"] = $"{invite.InvitedPlayerUserName} is already in a campaign.";

                    return View();
                }

                // Check if basic player has a invite pending
                int inviteNumber = await CampaignInvitesDB.HasInviteAsync(_context, basicPlayer);

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

                if (response.IsSuccessStatusCode)
                {
                    // If the email was successfully sent, add the invite to the database
                    await CampaignInvitesDB.CreateInviteAsync(_context, invite);

                    ViewData["InviteStatus"] = $"Invite was successfully sent to {invite.InvitedPlayerUserName}";

                    // Go back to index
                    return RedirectToAction("Index");
                }
            }

            // If we got this far redisplay form
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Basic Player")]
        public IActionResult AcceptInvite()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Basic Player")]
        public async Task<IActionResult> AcceptInviteAsync(InviteCode invite)
        {
            if (ModelState.IsValid)
            {
                // Get current user
                IdentityUser currentUser = await _userManager.GetUserAsync(User);

                // Get targeted invite
                CampaignInvites targetedInvite = await CampaignInvitesDB.GetInviteAsync(_context, invite.Code, currentUser);

                if (targetedInvite == null) // if true, there is no campaign with the entered inviteCode
                {
                    ViewData["Invite"] = "Could not find a campaign with that invite code. Check invite code and try again.";

                    return View();
                }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                else if (!targetedInvite.InvitedPlayerUserName.Equals(currentUser.UserName))
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                {
                    ViewData["NotInvited"] = "You were not invited to this campaign!";
                    return View();
                }

                else
                {
                    // Add the player to the campaign

                    // Get the desired campaign
                    Campaigns campaign = await CampaignDB.GetCampaignByOwnerAsync(_context, targetedInvite.DungeonMaster);

                    // Add player to campaign
                    CampaignPlayers campaignPlayer = await CampaignDB.JoinCampaignAsync(_context, campaign, currentUser);

                    // Delete the invite from database
                    _context.Entry(targetedInvite).State = EntityState.Deleted;

                    await _context.SaveChangesAsync(); // Save changes to the database
                }
            }

            return RedirectToAction("Index", "Campaign");
        }

        [HttpGet]
        [Authorize(Roles = "Dungeon Master")]
        public async Task<IActionResult> DeleteInvite(string invitedPlayerUserName) // Delete BasicPlayer from campaign
        {
            // Get the selected invite to delete
            CampaignInvites invite = await CampaignInvitesDB.GetInviteByNameAsync(_context, invitedPlayerUserName);

            // Return the invite to display is
            return View(invite);
        }

        [HttpPost]
        [ActionName("DeleteInvite")]
        [Authorize(Roles = "Dungeon Master")]
        public async Task<IActionResult> DeleteInviteConfirmed(string invitedPlayerUserName)
        {
            // Get the selected invite to delete
            CampaignInvites invite = await CampaignInvitesDB.GetInviteByNameAsync(_context, invitedPlayerUserName);

            _context.Entry(invite).State = EntityState.Deleted; // Delete the selected invite from database

            // Save changes to the db
            await _context.SaveChangesAsync();

            ViewData["DeleteInvite"] = "Invite has been successfully deleted";

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Dungeon Master")]
        public async Task<IActionResult> ResendInvite(string invitedPlayerUserName)
        {
            // Get the selected invite
            CampaignInvites invite = await CampaignInvitesDB.GetInviteByNameAsync(_context, invitedPlayerUserName);

            // Return it to the view
            return View(invite);
        }

        [HttpPost]
        [ActionName("ResendInvite")]
        [Authorize(Roles = "Dungeon Master")]
        public async Task<IActionResult> ResendInviteConfirmed(int inviteCode)
        {
            // Get current user
            IdentityUser currentUser = await _userManager.GetUserAsync(User);

            // Get invite 
            CampaignInvites invite = await CampaignInvitesDB.GetInviteAsync(_context, inviteCode, currentUser);

            // Get basic player 
            IdentityUser basicPlayer = await _userManager.FindByNameAsync(invite.InvitedPlayerUserName);

            // Resend the invite
            // If we got this far we can start the invite process
            // Send the targeted BasicPlayer the invite and invite code
            string toEmail = basicPlayer.Email;
            string fromEmail = "dndmanager.noreply@gmail.com";
            string subject = "Campaign Invite";
            string body = invite.DungeonMaster + " has invited you to a campaign! \n\n Your invite code is: " + invite.InviteCode;
            string htmlContent = "";

            var response = await _emailProvider.SendEmailAsync(basicPlayer.UserName, toEmail, fromEmail, subject, body, htmlContent);

            if (response.IsSuccessStatusCode)
            {
                ViewData["ResendInvite"] = "Invite has been successfully resent";

                return RedirectToAction("Index");
            }

            ViewData["ResendInvite"] = "Could not resend invite. Please try again later.";
            return View();
        }
    }
}
