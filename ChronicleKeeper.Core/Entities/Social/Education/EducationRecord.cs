using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Economy;
using ChronicleKeeper.Core.Entities.Social.Structure;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Education
{
    public class EducationRecord : ILoreEntity
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

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Degree { get; set; } = string.Empty; // Bachelor's, Military Rank, Wizard Certification
        //[ForeignKey("Character")]
        public int? CharacterId { get; set; }
        public Character? Character { get; set; }

        //[ForeignKey("School")]
        public int? SchoolId { get; set; } // If it was a school
        public School? School { get; set; }

        //[ForeignKey("University")]
        public int? UniversityId { get; set; } // If it was a university
        public University? University { get; set; }

        //[ForeignKey("Guild")]
        public int? GuildId { get; set; } // If it was a trade guild apprenticeship
        public Guild? Guild { get; set; }

    }

}
