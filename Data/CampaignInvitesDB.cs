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

        /// <summary>
        /// Check if targeted basic player is in a campaign
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="basicPlayer"></param>
        /// <returns>true if BasicPlayer is already in a campaign and return false if not in a campaign</returns>
        public static async Task<bool> IsInCampaign(ApplicationDbContext _context, IdentityUser basicPlayer)
        {
             int participatingCampaigns = await (from CampaignPlayers in _context.CampaignPlayers
                                                where CampaignPlayers.BasicPlayer == basicPlayer
                                                select CampaignPlayers).CountAsync();

            // BasicPlayer is only allowed to participate in 1 campaign at a time
            if(participatingCampaigns > 0)
            {
                return true;
            }

            else // If participatingCampaigns is < 1, it means that the BasicPlayer is not in a campagin
            {
                return false;
            }
        }
    }
}
