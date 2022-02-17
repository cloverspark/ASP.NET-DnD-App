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
    public class CampaignInvitesDB
    {
        /// <summary>
        /// Send an invite to a BasicPlayer to join the current DungeonMaster's campaign by their user name
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="invite">contains dungeonMaster, invite code, and targeted BasicPlayer</param>
        /// <returns>the successfully sent invite</returns>
        public static async Task<CampaignInvites> SendInvite(ApplicationDbContext _context, CampaignInvites invite)
        {
            _context.CampaignInvites.Add(invite);
            await _context.SaveChangesAsync();
            return invite;
        }

        public static async Task<List<CampaignInvites>> GetAllCampaignInvites(ApplicationDbContext _context, IdentityUser dungeonMaster)
        {
            return await (from CampaignInvites in _context.CampaignInvites
                          where CampaignInvites.DungeonMaster == dungeonMaster
                          select CampaignInvites).ToListAsync();
        }

        /// <summary>
        /// Get an invite with the invite code
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="inviteCode"></param>
        /// <returns></returns>
        public static async Task<CampaignInvites> GetInvite(ApplicationDbContext _context, int inviteCode)
        {
            try
            {                           // Ask why I have to do this to get DungeonMaster
                IdentityUser person = await (from CampaignInvites in _context.CampaignInvites
                                             where CampaignInvites.InviteCode == inviteCode
                                             select CampaignInvites.DungeonMaster).SingleAsync();
                CampaignInvites campaign = await (from CampaignInvites in _context.CampaignInvites
                                                  where CampaignInvites.InviteCode == inviteCode
                                                  select CampaignInvites).SingleAsync();
                return campaign;
            }
            catch(InvalidOperationException) // If there is no invite with that code return null
            {
                return null;
            }
        }

        /// <summary>
        /// Get the number of invites tied to the current BasicPlayer
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="basicplayer"></param>
        /// <returns>found invites</returns>
        public static async Task<int> HasInvite(ApplicationDbContext _context, IdentityUser basicplayer)
        {
            int invites = await (from CampaignInvites in _context.CampaignInvites
                                                    where CampaignInvites.InvitedPlayerUserName == basicplayer.UserName
                                                    select CampaignInvites).CountAsync();

            return invites;
        }
    }
}
