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
        public Campaigns CampaignsId { get; set; }

        public IdentityUser BasicPlayer { get; set; }
    }
}
