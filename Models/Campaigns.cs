using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_DnD_App.Models
{
    // When joining a campaign, the BasicPlayer's entry will be tied to the DungeonMatser. For viewing who is all in the same campaign, just search for accounts
    // tied to the desired DungeonMatser
    public class Campaigns
    {
        [Key]
        [Required]
        public int CampaignId { get; set; } 

        [Required]
        public IdentityUser DungeonMaster { get; set; } // If you get every entry by the DungeonMaster than you can find every BasicPlayer in a campaign 
    }
}
