using ChronicleKeeper.Core.Entities.Base;

namespace ChronicleKeeper.Core.Entities.HistoryTimelines
{
    /// <summary>
    /// A named era/period of a world's history (Name from LoreEntity, e.g. "Golden Age").
    /// </summary>
    public class Timeline : LoreEntity
    {
        public virtual ICollection<TimelineEvent> Events { get; set; } = new List<TimelineEvent>();

        //public int HistoryId { get; set; } // TODO: Uncomment when History entity is revived
        //public History History { get; set; } = null!;
    }
}
