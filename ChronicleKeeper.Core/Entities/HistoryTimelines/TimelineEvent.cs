using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography;
using System.ComponentModel.DataAnnotations;
//using ChronicleKeeper.Core.Entities.Social.Cultures;
//using ChronicleKeeper.Core.Entities.Social.Military;

namespace ChronicleKeeper.Core.Entities.HistoryTimelines
{
    public class TimelineEvent : LoreEntity
    {
        [Required]
        public int TimelineId { get; set; }
        public virtual Timeline? Timeline { get; set; }

        /// <summary>Freeform in-world date — fictional calendars don't fit DateTime (e.g. "Year 512, Third Age").</summary>
        public string Date { get; set; } = string.Empty;
        /// <summary>Deterministic ordering of events inside a timeline.</summary>
        public int SortOrder { get; set; }

        /// <summary>Optional freeform era/period label used to group events on the timeline (e.g. "Third Age").</summary>
        public string Era { get; set; } = string.Empty;

        public string Consequences { get; set; } = string.Empty; // Impact on history
        public bool IsMajorEvent { get; set; } // Determines historical importance

        /// <summary>Where the event took place (optional pointer, SetNull).</summary>
        public int? LocationId { get; set; }
        public virtual Location? Location { get; set; }

        /// <summary>Characters involved in the event (many-to-many via join entity).</summary>
        public virtual ICollection<TimelineEventCharacter> InvolvedCharacters { get; set; } = new List<TimelineEventCharacter>();

        //public int? BattleId { get; set; } // TODO: Uncomment when Battle entity is revived
        //public Battle? Battle { get; set; }
        //public int? FolkloreId { get; set; } // TODO: Uncomment when Folklore entity is revived
        //public Folklore? Folklore { get; set; }
    }
}
