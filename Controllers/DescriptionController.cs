using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_DnD_App.Controllers
{
    public class DescriptionController : Controller
    {
        // GET: DescriptionController
        public ActionResult Index()
        {
            return View();
        }

        // GET: DescriptionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DescriptionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DescriptionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DescriptionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DescriptionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DescriptionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DescriptionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
