using System.ComponentModel.DataAnnotations;

namespace ASP.NET_DnD_App.Models
{
    public class Character
    {
        [Key]
        [Required]
        public int CharacterId { get; set; }

        [Required]
        public string? CharacterName { get; set; }

        public string? ClassName {  get; set;}

        public int DescriptionId { get; set; }

        public int CharacterSheetId { get; set; }
    }
}
