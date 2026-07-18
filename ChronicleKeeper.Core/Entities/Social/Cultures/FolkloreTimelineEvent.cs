using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    /// <summary>Join: Folklore ↔ TimelineEvent (events this folklore relates to). Composite PK, not a LoreEntity.</summary>
    public class FolkloreTimelineEvent
    {
        public int FolkloreId { get; set; }
        public virtual Folklore? Folklore { get; set; }

        public int TimelineEventId { get; set; }
        public virtual TimelineEvent? TimelineEvent { get; set; }
    }
}
