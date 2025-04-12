using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Structure;
using ChronicleKeeper.Core.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Education
{
    public class University : ILoreEntity
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

        public bool FocusesOnScience { get; set; } // Engineering, Medicine, Physics
        public bool FocusesOnMagic { get; set; } // Magical research
        public bool FocusesOnPhilosophy { get; set; } // Arts and humanities
        public bool FocusesOnMilitaryStudies { get; set; } // War strategy academies

        //[ForeignKey("EducationSystem")]
        public int EducationSystemId { get; set; }
        public EducationSystem EducationSystem { get; set; } = null!;

        //public ICollection<Character> Students { get; set; } = new List<Character>();
        //public ICollection<Character> Professors { get; set; } = new List<Character>();
        public ICollection<EducationRecord> Alumni { get; set; } = new List<EducationRecord>();
    }
}
