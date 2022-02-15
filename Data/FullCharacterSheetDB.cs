using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASP.NET_DnD_App.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ASP.NET_DnD_App.Data
{
    public class FullCharacterSheetDB
    {
        /// <summary>
        /// Returns the total count of FullCharacterSheet database wide. This method will only be used by Admins
        /// </summary>
        /// <param name="_context">Database context to use</param>
        public static async Task<int> GetTotalCharactersInDBAsync(ApplicationDbContext _context)
        {
            return await (from c in _context.FullCharacterSheet
                          select c).CountAsync();
        }

        /// <summary>
        /// Returns the total count of FullCharacterSheet owned by the current user
        /// </summary>
        /// <param name="_context">Database context to use</param>
        public static async Task<int> GetUsersTotalCharactersAsync(ApplicationDbContext _context, IdentityUser characterOwner)
        {
            return await (from c in _context.FullCharacterSheet
                          where c.CharacterOwner == characterOwner
                          select c).CountAsync();
        }

        /// <summary>
        /// Get a page worth of products
        /// </summary>
        /// <param name="_context">Database context to use</param>
        /// <param name="pageSize">The number of products per page</param>
        /// <param name="pageNum">Page of FullCharacterSheet to return</param>
        public static async Task<List<FullCharacterSheet>> GetCharactersAsync(ApplicationDbContext _context, int pageSize, int pageNum, IdentityUser characterOwner)
        {
            return await (from c in _context.FullCharacterSheet
                          where c.CharacterOwner == characterOwner
                          orderby c.CharacterName ascending
                          select c)
                        .Skip(pageSize * (pageNum - 1)) // Skip() must be before Take()
                        .Take(pageSize)
                        .ToListAsync();
        }

        /// <summary>
        /// Add a character to the database
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="c">Newly created character</param>
        /// <returns></returns>
        public static async Task<FullCharacterSheet> AddCharacterAsync(ApplicationDbContext _context, FullCharacterSheet c)
        { 
            // Add to DB
            _context.FullCharacterSheet.Add(c);
            await _context.SaveChangesAsync();
            return c;
        }

        /// <summary>
        /// Get characters tide to the current user
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="CharacterSheetId"></param>
        /// <param name="characterOwner"></param>
        /// <returns></returns>
        public static async Task<FullCharacterSheet> GetCharacterAsync(ApplicationDbContext _context, int CharacterSheetId, IdentityUser characterOwner)
        {
            FullCharacterSheet c = await (from FullCharacterSheet in _context.FullCharacterSheet
                                          where FullCharacterSheet.CharacterSheetId == CharacterSheetId
                                          && FullCharacterSheet.CharacterOwner == characterOwner // Only get characters that belong to the owner
                                          select FullCharacterSheet).SingleAsync();
            return c;
        }
        
    }
}
