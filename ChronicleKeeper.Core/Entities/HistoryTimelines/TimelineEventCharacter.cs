using ChronicleKeeper.Core.Entities.Characters;

namespace ChronicleKeeper.Core.Entities.HistoryTimelines
{
    /// <summary>Join entity: a character involved in a timeline event (many-to-many).</summary>
    public class TimelineEventCharacter
    {
        public int TimelineEventId { get; set; }
        public virtual TimelineEvent? TimelineEvent { get; set; }

        public int CharacterId { get; set; }
        public virtual Character? Character { get; set; }
    }
}
