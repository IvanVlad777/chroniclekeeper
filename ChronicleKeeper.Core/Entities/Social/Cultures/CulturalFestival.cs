using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class CulturalFestival : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        // Fictional in-world date — plain string stopgap (see custom-calendar TODO), like TimelineEvent.Date.
        public string? StartDate { get; set; }
        public int DurationDays { get; set; } // Festival length
        public string Activities { get; set; } = string.Empty; // Events, customs, dances
        public bool IsNationalHoliday { get; set; } // Whether the festival is an official holiday

        // Optional location — SetNull.
        public int? LocationId { get; set; }
        public virtual Location? Location { get; set; }

        public int CultureId { get; set; }
        public virtual Culture? Culture { get; set; }
    }
}
