namespace ASP.NET_DnD_App.Models
{
    public class FullCharacterSheet
    {
        public int CharacterId { get; set; }

        public string? CharacterName { get; set; }

        public string? ClassName { get; set; }

        public int DescriptionId { get; set; }

        public int CharacterSheetId { get; set; }

        public string? HairStyle { get; set; }

        public string? HairColor { get; set; }

        public string? EyeColor { get; set; }

        public string? SkinType { get; set; }

        public string? RaceName { get; set; }
    }
}
