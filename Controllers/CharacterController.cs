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

            int numProducts = await FullCharacterSheetDB.GetTotalCharactersAsync(_context);
            int totalPages = (int)Math.Ceiling((double)numProducts / PageSize);
            ViewData["MaxPage"] = totalPages;

            List<FullCharacterSheet> products =
                await FullCharacterSheetDB.GetCharactersAsync(_context, PageSize, pageNum);

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
        public async Task<IActionResult> Create(FullCharacterSheet c)
        {
            if (ModelState.IsValid)
            {
                await FullCharacterSheetDB.AddCharacterAsync(_context, c);

                TempData["Message"] = $"{c.CharacterName} was added successfully";

                // redirect back to catalog page
                return RedirectToAction("Index");
            }

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            FullCharacterSheet p = await FullCharacterSheetDB.GetCharacterAsync(_context, id);
            // pass product to view
            return View(p);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FullCharacterSheet c)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(c).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                ViewData["Message"] = "Character Sheet updated successfully";
            }

            return View(c);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            FullCharacterSheet c = await FullCharacterSheetDB.GetCharacterAsync(_context, id);
            return View(c);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            FullCharacterSheet c = await FullCharacterSheetDB.GetCharacterAsync(_context, id);

            _context.Entry(c).State = EntityState.Deleted;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
