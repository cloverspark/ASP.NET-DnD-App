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

        // Display current user's characters in pages of 8 (Each page will display 8 characters)
        public async Task<IActionResult> Index(int? id)
        {
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator
            int pageNum = id ?? 1;
            const int PageSize = 8;
            ViewData["CurrentPage"] = pageNum;

            // Get current user
            IdentityUser currentUser = await _userManager.GetUserAsync(User);

            int numCharacters = await FullCharacterSheetDB.GetUsersTotalCharactersAsync(_context, currentUser); // Get the count of the current User's characters 
            int totalPages = (int)Math.Ceiling((double)numCharacters / PageSize); // Davide the total number of characters by 8  
            ViewData["MaxPage"] = totalPages; 

            List<FullCharacterSheet> characters =
                await FullCharacterSheetDB.GetCharactersAsync(_context, PageSize, pageNum, currentUser); // Get a list of all the User's characters with the correct 
                                                                                                         // amount to be displayed per page

            // Send list of products to view to be displayed
            return View(characters);
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
                // Get current user
                IdentityUser currentUser = await _userManager.GetUserAsync(User);

                // Assign character to current user
                c.CharacterOwner = currentUser;

                await FullCharacterSheetDB.AddCharacterAsync(_context, c); // Add character to database

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

            FullCharacterSheet p = await FullCharacterSheetDB.GetCharacterAsync(_context, id, currentUser); // Get selected character

            // Pass character to view
            return View(p);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FullCharacterSheet c)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(c).State = EntityState.Modified; // Update selected character with what was entered on the form

                await _context.SaveChangesAsync(); // Save changes to the database

                ViewData["Message"] = "Character Sheet updated successfully";
            }

            // Redirect to main character page
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            // Get current user
            IdentityUser currentUser = await _userManager.GetUserAsync(User); 

            FullCharacterSheet c = await FullCharacterSheetDB.GetCharacterAsync(_context, id, currentUser); // Get selected character

            // Return selected character to the current page
            return View(c);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Get current user
            IdentityUser currentUser = await _userManager.GetUserAsync(User);

            FullCharacterSheet c = await FullCharacterSheetDB.GetCharacterAsync(_context, id, currentUser); // Get selected character

            _context.Entry(c).State = EntityState.Deleted; // Delete the selected character from database

            await _context.SaveChangesAsync(); // Save changes to the database

            // Redirect to main character page
            return RedirectToAction("Index");
        }
    }
}
