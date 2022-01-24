using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_DnD_App.Models
{
    public class FullCharacterSheet
    {
        [Key]
        public int CharacterSheetId { get; set; }

        [Required]
        public string? CharacterName { get; set; }

        [Required]
        public string? ClassName { get; set; }

        [Required]
        public string? HairStyle { get; set; }

        [Required]
        public string? HairColor { get; set; }

        [Required]
        public string? EyeColor { get; set; }

        [Required]
        public string? SkinType { get; set; }

        [Required]
        public string? RaceName { get; set; }

        [Required]
        public IdentityUser CharacterOwner { get; set; }
    }
}
