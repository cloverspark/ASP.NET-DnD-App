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
        // Get a campaign member by playerId
        public static async Task<CampaignPlayers> GetCampaignPlayerAsync(ApplicationDbContext _context, int playerId)
        {
            return await (from CampaignPlayers in _context.CampaignPlayers
                          where CampaignPlayers.PlayerId == playerId
                          select CampaignPlayers).Include(nameof(CampaignPlayers.BasicPlayer)).SingleOrDefaultAsync();
        }

        // Get a campaign. This is only called when someone accepts an invite
        public static async Task<Campaigns> GetCampaignByOwnerAsync(ApplicationDbContext _context, IdentityUser dungeonMaster)
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
                await CreateCampaignAsync(_context, newCampaign);

                return newCampaign;
            }
        }

        /// <summary>
        /// Get campaign id if you're in a campaign
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="user"></param>
        /// <returns>campaign id</returns>
        public static async Task<int> GetCampaignIdByPlayerAsync(ApplicationDbContext _context, IdentityUser user)
        {
                return await (from CampaignPlayers in _context.CampaignPlayers
                              where CampaignPlayers.BasicPlayer.Id == user.Id
                              select CampaignPlayers.CampaignId).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Get campaign id if you're a dungeon master
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="user"></param>
        /// <returns>campaign id</returns>
        public static async Task<int> GetCampaignIdByDungeonMasterAsync(ApplicationDbContext _context, IdentityUser user)
        {
            return await (from Campaigns in _context.Campaigns
                          where Campaigns.DungeonMaster.Id == user.Id
                          select Campaigns.CampaignId).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Get all players in a specific campaign by an given campaign id
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="campignId"></param>
        /// <returns>list of campaign members</returns>
        public static async Task<List<CampaignPlayers>> GetCampaignMembersByIdAsync(ApplicationDbContext _context, int campignId)
        {

            List<CampaignPlayers> campaignPlayers = await (from CampaignPlayers in _context.CampaignPlayers
                                                           where CampaignPlayers.CampaignId == campignId
                                                           select CampaignPlayers).Include(nameof(CampaignPlayers.BasicPlayer)).ToListAsync();

            return campaignPlayers;
        }

        // Get the Dungeon Master for a specific campaign
        public static async Task<IdentityUser> GetDungeonMasterAsync(ApplicationDbContext _context, int campaignId)
        {
            return await (from Campaigns in _context.Campaigns
                          where Campaigns.CampaignId == campaignId
                          select Campaigns.DungeonMaster).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Create a new campaign
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="camapign"></param>
        /// <returns></returns>
        public static async Task<Campaigns> CreateCampaignAsync(ApplicationDbContext _context, Campaigns camapign)
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
        public static async Task<CampaignPlayers> JoinCampaignAsync(ApplicationDbContext _context, Campaigns targetedCampaign, IdentityUser basicPlayer)
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
        public static async Task<bool> IsInCampaignAsync(ApplicationDbContext _context, IdentityUser basicPlayer)
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
