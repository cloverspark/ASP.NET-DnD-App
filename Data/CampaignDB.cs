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
    public class CampaignDB
    {
        // Get a campaign
        public static async Task<Campaigns> GetCampaign(ApplicationDbContext _context, IdentityUser dungeonMaster)
        {
            try
            {
                Campaigns campaign = await (from Campaigns in _context.Campaigns
                                            where Campaigns.DungeonMaster == dungeonMaster
                                            select Campaigns).SingleAsync();
                return campaign; 
            }
            catch (InvalidOperationException) // if there is no campaign with that dungeon master, create one
            {
                Campaigns newCampaign = new Campaigns
                {
                    DungeonMaster = dungeonMaster,
                };

                // Create a new campaign
                await CreateCampaign(_context, newCampaign);

                return newCampaign;
            }
        }

        /// <summary>
        /// Create a new campaign
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="camapign"></param>
        /// <returns></returns>
        public static async Task<Campaigns> CreateCampaign(ApplicationDbContext _context, Campaigns camapign)
        {
            _context.Campaigns.Add(camapign);
            await _context.SaveChangesAsync();

            return camapign;
        }

        /// <summary>
        /// Add a basic player to a campaign
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="targetedCampaign">campaign to join</param>
        /// <param name="basicPlayer">player that's joining the campaign</param>
        /// <returns></returns>
        public static async Task<CampaignPlayers> JoinCampaign(ApplicationDbContext _context, Campaigns targetedCampaign, IdentityUser basicPlayer)
        {
            CampaignPlayers newPlayer = new CampaignPlayers
            {
                BasicPlayer = basicPlayer,
                CampaignId = targetedCampaign.CampaignId
            };

            _context.CampaignPlayers.Add(newPlayer);
            await _context.SaveChangesAsync();

            return newPlayer;
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
            if (participatingCampaigns > 0)
            {
                return true;
            }

            else // If participatingCampaigns is < 1, it means that the BasicPlayer is not in a campaign
            {
                return false;
            }
        }
    }
}
