using ChronicleKeeper.Core.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Social.Education
{
    public class University : LoreEntity
    {
        public bool FocusesOnScience { get; set; } // Engineering, Medicine, Physics
        public bool FocusesOnMagic { get; set; } // Magical research
        public bool FocusesOnPhilosophy { get; set; } // Arts and humanities
        public bool FocusesOnMilitaryStudies { get; set; } // War strategy academies

        [Required]
        public int EducationSystemId { get; set; }
        public virtual EducationSystem EducationSystem { get; set; } = null!;

        public virtual ICollection<EducationRecord> Alumni { get; set; } = new List<EducationRecord>();
        public virtual ICollection<UniversityMajor> Majors { get; set; } = new List<UniversityMajor>();

        //public virtual ICollection<Characters.Character> Students { get; set; } = new List<Characters.Character>(); // TODO: Uncomment when Character many-to-many cross-links are revived
        //public virtual ICollection<Characters.Character> Professors { get; set; } = new List<Characters.Character>(); // TODO: Uncomment when Character many-to-many cross-links are revived
    }
}
