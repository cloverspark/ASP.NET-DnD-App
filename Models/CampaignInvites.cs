using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_DnD_App.Models
{
    // When a DuneonMaster sends an invite to a campaign to a BasicPlayer the invite information gets put into this 
    // table until the BasicPlayer accepts this invite. (Invite info will be deleted from this table when
    // invite was accepted)
    public class CampaignInvites
    {
        [Key]
        public int InviteId { get; set; }

        public IdentityUser? DungeonMaster { get; set; } // Dungeon master of the campaign

        public int InviteCode { get; set; } // Use this code to join a game. It is only used to let invited people to the campaign

        [Required]
        public string? InvitedPlayerUserName { get; set; } // This is used to check if the user who gets the code was actually invited 
    }
}
