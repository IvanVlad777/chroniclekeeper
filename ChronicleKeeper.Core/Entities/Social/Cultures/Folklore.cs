using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class Folklore : ILoreEntity
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

        public string Story { get; set; } = string.Empty; // Description of the legend or folktale
        public string Moral { get; set; } = string.Empty; // Lesson or message of the story
        public bool IsHistorical { get; set; } // True if based on actual events

        //[ForeignKey("Culture")]
        public int CultureId { get; set; }
        public Culture Culture { get; set; } = null!;

        public ICollection<TimelineEvent> RelatedEvents { get; set; } = new List<TimelineEvent>();

        public ICollection<SapientSpecies> OriginatedFromSpecies { get; set; } = new List<SapientSpecies>(); // ✅ Which species created this folklore
    }

}
