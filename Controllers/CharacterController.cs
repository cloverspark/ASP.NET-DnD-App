using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ASP.NET_DnD_App.Models;
using ASP.NET_DnD_App.Data;

namespace ASP.NET_DnD_App.Controllers
{
    public class CharacterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CharacterController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: CharacterController
        public async Task<IActionResult> Index(int? id)
        {
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator
            int pageNum = id ?? 1;
            const int PageSize = 8;
            ViewData["CurrentPage"] = pageNum;

            int numProducts = await FullCharacterSheetDB.GetTotalProductsAsync(_context);
            int totalPages = (int)Math.Ceiling((double)numProducts / PageSize);
            ViewData["MaxPage"] = totalPages;

            List<FullCharacterSheet> products =
                await FullCharacterSheetDB.GetProductsAsync(_context, PageSize, pageNum);

            // Send list of products to view to be displayed
            return View(products);
        }

        // GET: CharacterController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CharacterController/Create
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

        // GET: CharacterController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CharacterController/Edit/5
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

        // GET: CharacterController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CharacterController/Delete/5
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
