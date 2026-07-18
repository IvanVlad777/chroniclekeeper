using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Religions
{
    public class HolySite : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public string Significance { get; set; } = string.Empty; // Why it's sacred (miracles, divine events)
        public bool IsPilgrimageDestination { get; set; } // If people travel here for religious reasons

        public int ReligionId { get; set; }
        public virtual Religion? Religion { get; set; }

        // Optional deity link — SetNull.
        public int? DeityId { get; set; }
        public virtual Deity? Deity { get; set; }

        // Required physical location — Restrict + delete-guard (Location is "in use").
        public int LocationId { get; set; }
        public virtual Location? Location { get; set; }
    }
}
