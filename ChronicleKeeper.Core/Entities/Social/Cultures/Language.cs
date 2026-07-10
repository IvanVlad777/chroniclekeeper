using ChronicleKeeper.Core.Entities.Base;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class Language : LoreEntity
    {
        public string WritingSystem { get; set; } = string.Empty; // Alphabet, Syllabary, Logographic
        public bool IsExtinct { get; set; } // If the language is no longer spoken
        public string Dialects { get; set; } = string.Empty; // Variants of the language

        public ICollection<Culture> Cultures { get; set; } = new List<Culture>();

        public virtual ICollection<LanguageNation> Nations { get; set; } = new List<LanguageNation>();
    }
}
