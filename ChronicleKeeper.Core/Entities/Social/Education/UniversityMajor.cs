using ChronicleKeeper.Core.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Social.Education
{
    public class UniversityMajor : LoreEntity
    {
        public string MajorName { get; set; } = string.Empty; // Arcane Studies, Engineering, Political Science
        public string DegreeLevel { get; set; } = string.Empty; // Bachelor's, Master's, Doctorate

        [Required]
        public int UniversityId { get; set; }
        public virtual University University { get; set; } = null!;

        // Character many-to-many (join entities below) — professors and students of this major.
        public virtual ICollection<UniversityMajorProfessor> Professors { get; set; } = new List<UniversityMajorProfessor>();
        public virtual ICollection<UniversityMajorStudent> Students { get; set; } = new List<UniversityMajorStudent>();
    }

    /// <summary>Join: UniversityMajor ↔ Character (professors). Composite PK, not a LoreEntity, Cascade both sides.</summary>
    public class UniversityMajorProfessor
    {
        public int UniversityMajorId { get; set; }
        public virtual UniversityMajor? UniversityMajor { get; set; }

        public int CharacterId { get; set; }
        public virtual Characters.Character? Character { get; set; }
    }

    /// <summary>Join: UniversityMajor ↔ Character (students). Composite PK, not a LoreEntity, Cascade both sides.</summary>
    public class UniversityMajorStudent
    {
        public int UniversityMajorId { get; set; }
        public virtual UniversityMajor? UniversityMajor { get; set; }

        public int CharacterId { get; set; }
        public virtual Characters.Character? Character { get; set; }
    }
}
