using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASP.NET_DnD_App.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ASP.NET_DnD_App.Data
{
    public class FullCharacterSheetDB
    {
        /// <summary>
        /// Returns the total count of FullCharacterSheet
        /// </summary>
        /// <param name="_context">Database context to use</param>
        public static async Task<int> GetTotalCharactersAsync(ApplicationDbContext _context)
        {
            return await (from c in _context.FullCharacterSheet
                          select c).CountAsync();
        }


        /// <summary>
        /// Get a page worth of products
        /// </summary>
        /// <param name="_context">Database context to use</param>
        /// <param name="pageSize">The number of products per page</param>
        /// <param name="pageNum">Page of FullCharacterSheet to return</param>
        public static async Task<List<FullCharacterSheet>> GetCharactersAsync(ApplicationDbContext _context, int pageSize, int pageNum)
        {
            return await (from c in _context.FullCharacterSheet
                          orderby c.CharacterName ascending
                          select c)
                        .Skip(pageSize * (pageNum - 1)) // Skip() must be before Take()
                        .Take(pageSize)
                        .ToListAsync();
        }

        public static async Task<FullCharacterSheet> AddCharacterAsync(ApplicationDbContext _context, FullCharacterSheet c)
        {
            // Add to DB
            _context.FullCharacterSheet.Add(c);
            await _context.SaveChangesAsync();
            return c;
        }
        public static async Task<FullCharacterSheet> GetCharacterAsync(ApplicationDbContext _context, int CharacterSheetId)
        {
            FullCharacterSheet c = await (from FullCharacterSheet in _context.FullCharacterSheet
                                          where FullCharacterSheet.CharacterSheetId == CharacterSheetId
                                          select FullCharacterSheet).SingleAsync();
            return c;
        }
        
    }
}
