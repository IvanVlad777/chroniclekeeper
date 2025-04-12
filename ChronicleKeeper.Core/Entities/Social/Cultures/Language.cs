using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Nationality;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class Language : ILoreEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public virtual History? History { get; set; }

        public string WritingSystem { get; set; } = string.Empty; // Alphabet, Syllabary, Logographic
        public bool IsExtinct { get; set; } // If the language is no longer spoken
        public string Dialects { get; set; } = string.Empty; // Variants of the language

        public ICollection<Culture> Cultures { get; set; } = new List<Culture>(); // ✅ Spoken by specific cultures
        public ICollection<Nation> Nations { get; set; } = new List<Nation>(); // ✅ Spoken by specific cultures
    }

}
