using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Professions;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class CorporateLeadership : LoreEntity
    {
        public string Position { get; set; } = string.Empty; // CEO, CFO, Board Member
        public double Salary { get; set; } // Annual salary in native currency
        public bool IsMajorShareholder { get; set; } // If they own significant company stock

        [Required]
        public int CorporationId { get; set; }
        public virtual Corporation Corporation { get; set; } = null!;

        public int? ProfessionId { get; set; } // Their professional background
        public virtual Profession? Profession { get; set; }

        public int? CharacterId { get; set; } // The character holding this position, if any
        public virtual Character? Character { get; set; }

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }
    }
}
