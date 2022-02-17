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
    }
}
