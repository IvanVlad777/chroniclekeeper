using ChronicleKeeper.Core.Entities.Base;
using System.ComponentModel.DataAnnotations;
//using ChronicleKeeper.Core.Entities.Characters;
//using ChronicleKeeper.Core.Entities.Geography;
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

        public string Consequences { get; set; } = string.Empty; // Impact on history
        public bool IsMajorEvent { get; set; } // Determines historical importance

        //public int? LocationId { get; set; } // TODO: Uncomment when the Location link is wired (delete behavior: SetNull)
        //public Location? Location { get; set; }
        //public ICollection<Character> InvolvedCharacters { get; set; } = new List<Character>(); // TODO: Uncomment when the join entity is added
        //public int? BattleId { get; set; } // TODO: Uncomment when Battle entity is revived
        //public Battle? Battle { get; set; }
        //public int? FolkloreId { get; set; } // TODO: Uncomment when Folklore entity is revived
        //public Folklore? Folklore { get; set; }
    }
}
