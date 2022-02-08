using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_DnD_App.Controllers
{
    public class CampaignController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
