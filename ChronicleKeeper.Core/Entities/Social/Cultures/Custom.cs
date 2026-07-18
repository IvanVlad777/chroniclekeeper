using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class Custom : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public bool IsUniversal { get; set; }

        // Optional — a universal custom may not belong to a specific culture.
        public int? CultureId { get; set; }
        public virtual Culture? Culture { get; set; }
    }
}
