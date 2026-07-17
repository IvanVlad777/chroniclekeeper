using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Military
{
    public class MilitaryRank : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public string RankTitle { get; set; } = string.Empty; // e.g., General, Captain, Lieutenant
        public int RankLevel { get; set; } // Higher numbers = higher rank

        // Compositional child of a unit — required FK, Cascade.
        public int MilitaryUnitId { get; set; }
        public virtual MilitaryUnit MilitaryUnit { get; set; } = null!;
    }
}
