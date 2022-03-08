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
            int campaignId = -1;

            if (await _userManager.IsInRoleAsync(currentUser, "Dungeon Master")) // If user is a Dungeon Master look on the Campaigns table
            {
                campaignId = await CampaignDB.GetCampaignIdByDungeonMasterAsync(_context, currentUser);
            }

            else // If user is a Basic Player look in CampaignPlayers table
            {
                campaignId = await CampaignDB.GetCampaignIdByPlayer(_context, currentUser);
            }

            // Get all your campaign members
            List<CampaignPlayers> allCampaignMembers = await CampaignDB.GetCampaignMembersByIdAsync(_context, campaignId);

            // Get the Dungeon Master
            dungeonMaster = await CampaignDB.GetDungeonMasterAsync(_context, campaignId);

            // Send dungeonMaster to the View
            ViewData["DungeonMaster"] = dungeonMaster;

            // Send campaignId to the View
            ViewData["CampaingId"] = campaignId;
            
            // If no campaign members just return the view
            return View(allCampaignMembers);
        }
    }
}
