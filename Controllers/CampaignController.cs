using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_DnD_App.Controllers
{
    public class CampaignController : Controller
    {
        public IActionResult Index() // View all players in the current DungeonMatser's campaign
        {
            return View();
        }

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

        public IActionResult InvitePlayer()
        {
            return View();
        }
    }
}
