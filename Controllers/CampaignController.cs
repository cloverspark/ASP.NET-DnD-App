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

        public async Task<IActionResult> IndexAsync()
        {
            // Get the current user and Dungeon Master
            IdentityUser currentUser = await _userManager.GetUserAsync(User);
            IdentityUser dungeonMaster;

            // Get campaign id
            int campaignId = await CampaignDB.GetCampaignIdByUser(_context, currentUser);

            // Get all your campaign members
            List<CampaignPlayers> allCampaignMembers = await CampaignDB.GetCampaignMembersByIdAsync(_context, campaignId);

            //if (campaignId >= 0) // If campaignId is greater than -1 means that user is in a campaign

                // Get the Dungeon Master
                dungeonMaster = await CampaignDB.GetDungeonMasterAsync(_context, campaignId);

                // Send dungeonMaster to the View
                ViewData["DungeonMaster"] = dungeonMaster;

                // Send campaignId to the View
                ViewData["CampaingId"] = campaignId;
            //}
            
            // If no campaign members just return the view
            return View(allCampaignMembers);
        }
    }
}
