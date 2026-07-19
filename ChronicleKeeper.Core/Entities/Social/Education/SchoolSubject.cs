using ChronicleKeeper.Core.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Social.Education
{
    public class SchoolSubject : LoreEntity
    {
        public string SubjectName { get; set; } = string.Empty; // Mathematics, History, Magic Theory
        public bool IsMandatory { get; set; } // Required for all students?

        [Required]
        public int SchoolId { get; set; }
        public virtual School School { get; set; } = null!;

        // Character many-to-many (join entity SchoolSubjectTeacher) — teachers of this subject.
        public virtual ICollection<SchoolSubjectTeacher> Teachers { get; set; } = new List<SchoolSubjectTeacher>();
    }

    /// <summary>Join: SchoolSubject ↔ Character (teachers). Composite PK, not a LoreEntity, Cascade both sides.</summary>
    public class SchoolSubjectTeacher
    {
        public int SchoolSubjectId { get; set; }
        public virtual SchoolSubject? SchoolSubject { get; set; }

        public int CharacterId { get; set; }
        public virtual Characters.Character? Character { get; set; }
    }
}
