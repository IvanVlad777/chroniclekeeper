using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Professions;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class CorporateLeadership : ILoreEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public virtual History? History { get; set; }

        public string Position { get; set; } = string.Empty; // CEO, CFO, Board Member
        public double Salary { get; set; } // Annual salary in native currency
        public bool IsMajorShareholder { get; set; } // If they own significant company stock

        //[ForeignKey("Corporation")]
        public int CorporationId { get; set; }
        public Corporation Corporation { get; set; } = null!;

        //[ForeignKey("Profession")]
        public int? ProfessionId { get; set; } // Their professional background
        public Profession? Profession { get; set; }
    }
}
