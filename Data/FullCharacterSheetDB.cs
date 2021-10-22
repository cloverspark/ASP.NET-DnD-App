using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASP.NET_DnD_App.Models;

namespace ASP.NET_DnD_App.Data
{
    public class FullCharacterSheetDB
    {
        /// <summary>
        /// Returns the total count of products
        /// </summary>
        /// <param name="_context">Database context to use</param>
        public static async Task<int> GetTotalProductsAsync(ApplicationDbContext _context)
        {
            return await (from p in _context.FullCharacterSheet
                          select p).CountAsync();
        }


        /// <summary>
        /// Get a page worth of products
        /// </summary>
        /// <param name="_context">Database context to use</param>
        /// <param name="pageSize">The number of products per page</param>
        /// <param name="pageNum">Page of products to return</param>
        public static async Task<List<FullCharacterSheet>> GetProductsAsync(ApplicationDbContext _context, int pageSize, int pageNum)
        {
            return await (from p in _context.FullCharacterSheet
                          orderby p.CharacterName ascending
                          select p)
                        .Skip(pageSize * (pageNum - 1)) // Skip() must be before Take()
                        .Take(pageSize)
                        .ToListAsync();
        }
    }
}
