using ChronicleKeeper.Core.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Social.Education
{
    /// <summary>
    /// TPH root: shared "Schools" table with TradeSchool (Professions namespace),
    /// distinguished by the "SchoolType" discriminator column.
    /// </summary>
    public class School : LoreEntity
    {
        public bool IsPublic { get; set; } // True = Free for all citizens
        public bool IsReligious { get; set; } // Theology-based education

        [Required]
        public int EducationSystemId { get; set; }
        public virtual EducationSystem EducationSystem { get; set; } = null!;

        public virtual ICollection<EducationRecord> Alumni { get; set; } = new List<EducationRecord>();
        public virtual ICollection<SchoolSubject> Subjects { get; set; } = new List<SchoolSubject>();

        public int? LocationId { get; set; }
        public virtual Geography.Location? Location { get; set; }

        public virtual ICollection<SchoolStudent> Students { get; set; } = new List<SchoolStudent>();
        public virtual ICollection<SchoolTeacher> Teachers { get; set; } = new List<SchoolTeacher>();
    }
}
