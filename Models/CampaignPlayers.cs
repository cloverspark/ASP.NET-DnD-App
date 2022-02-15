using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_DnD_App.Models
{
    public class CampaignPlayers
    {
        [Key]
        [Required]
        public int PlayerId { get; set; } 

        [Required]
        public int CampaignId { get; set; } // Link to a hosted campaign in Campaign table

        public IdentityUser BasicPlayer { get; set; } // Player that belongs to a campaign
    }
}
