using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class Folklore : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public string Story { get; set; } = string.Empty; // Description of the legend or folktale
        public string Moral { get; set; } = string.Empty; // Lesson or message of the story
        public bool IsHistorical { get; set; } // True if based on actual events

        public int CultureId { get; set; }
        public virtual Culture? Culture { get; set; }

        // M:N with TimelineEvent (join entity) — owner side.
        public virtual ICollection<FolkloreTimelineEvent> RelatedEvents { get; set; } = new List<FolkloreTimelineEvent>();

        // M:N with SapientSpecies (join entity) — owner side. Which species created this folklore.
        public virtual ICollection<FolkloreSapientSpecies> OriginatedFromSpecies { get; set; } = new List<FolkloreSapientSpecies>();
    }
}
