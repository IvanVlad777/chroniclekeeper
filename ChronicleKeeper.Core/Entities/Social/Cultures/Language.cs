using ChronicleKeeper.Core.Entities.Base;
//using ChronicleKeeper.Core.Entities.Social.Nationality;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class Language : LoreEntity
    {
        public string WritingSystem { get; set; } = string.Empty; // Alphabet, Syllabary, Logographic
        public bool IsExtinct { get; set; } // If the language is no longer spoken
        public string Dialects { get; set; } = string.Empty; // Variants of the language

        public ICollection<Culture> Cultures { get; set; } = new List<Culture>();

        // TODO: Many-to-many cross-link to Nation — deferred to a dedicated pass (see Culture.cs).
        //public ICollection<Nation> Nations { get; set; } = new List<Nation>();
    }
}
