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

        // Professors/Students: TODO: Uncomment when Character many-to-many cross-links are revived
        //public virtual ICollection<Characters.Character> Professors { get; set; } = new List<Characters.Character>();
        //public virtual ICollection<Characters.Character> Students { get; set; } = new List<Characters.Character>();
    }
}
