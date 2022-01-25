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
    public class CharacterController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;


        public CharacterController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: CharacterController
        public async Task<IActionResult> Index(int? id)
        {
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator
            int pageNum = id ?? 1;
            const int PageSize = 8;
            ViewData["CurrentPage"] = pageNum;

            // Get current user
            IdentityUser currentUser = await _userManager.GetUserAsync(User);

            int numCharacters = await FullCharacterSheetDB.GetUsersTotalCharactersAsync(_context, currentUser);
            int totalPages = (int)Math.Ceiling((double)numCharacters / PageSize);
            ViewData["MaxPage"] = totalPages;

            List<FullCharacterSheet> characters =
                await FullCharacterSheetDB.GetCharactersAsync(_context, PageSize, pageNum, currentUser);

            // Send list of products to view to be displayed
            return View(characters);
        }

        // GET: CharacterController/Create
        public ActionResult Create()
        {
            bool isva = ModelState.IsValid;
            return View();
        }

        // POST: CharacterController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FullCharacterSheet c)
        {
            if (ModelState.IsValid)
            {
                // Get current user
                IdentityUser currentUser = await _userManager.GetUserAsync(User);

                // Assign character to current user
                c.CharacterOwner = currentUser;

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
            // Get current user
            IdentityUser currentUser = await _userManager.GetUserAsync(User);

            FullCharacterSheet p = await FullCharacterSheetDB.GetCharacterAsync(_context, id, currentUser);
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

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            // Get current user
            IdentityUser currentUser = await _userManager.GetUserAsync(User);

            FullCharacterSheet c = await FullCharacterSheetDB.GetCharacterAsync(_context, id, currentUser);
            return View(c);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Get current user
            IdentityUser currentUser = await _userManager.GetUserAsync(User);

            FullCharacterSheet c = await FullCharacterSheetDB.GetCharacterAsync(_context, id, currentUser);

            _context.Entry(c).State = EntityState.Deleted;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
