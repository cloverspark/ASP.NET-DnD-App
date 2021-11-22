using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_DnD_App.Controllers
{
    public class RacesController : Controller
    {
        public IActionResult Elves()
        {
            return View();
        }
        public IActionResult Dwarves()
        {
            return View();
        }
        public IActionResult Halfings()
        {
            return View();

        }
        public IActionResult Humans()
        {
            return View();
        }
        public IActionResult Dragonborn()
        {
            return View();
        }
        public IActionResult Gnomes()
        {
            return View();
        }
        public IActionResult Half_elves()
        {
            return View();
        }
        public IActionResult Half_Orcs()
        {
            return View();
        }
        public IActionResult Tieflings()
        {
            return View();
        }
    }
}
