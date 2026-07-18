using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Religions
{
    public class ReligiousFestival : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        // Fictional in-world date — plain string stopgap, like TimelineEvent.Date / CulturalFestival.StartDate.
        public string? StartDate { get; set; }
        public int DurationDays { get; set; } // Length of festival
        public string Traditions { get; set; } = string.Empty; // Rituals and celebrations
        public bool IsPilgrimageEvent { get; set; } // If travel to a holy site is part of the festival

        public int ReligionId { get; set; }
        public virtual Religion? Religion { get; set; }

        // Optional holy site — SetNull.
        public int? HolySiteId { get; set; }
        public virtual HolySite? HolySite { get; set; }
    }
}
