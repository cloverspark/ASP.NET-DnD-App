using System.ComponentModel.DataAnnotations;

namespace ASP.NET_DnD_App.Models
{
    public class Description
    {
        [Key]
        [Required]
        public int DescriptionId { get; set; }

        public string HairStyle { get; set; }

        public string HairColor { get; set; }

        public string EyeColor { get; set; }

        public string SkinType { get; set; }

        public string RaceName { get; set; }
    }
}
