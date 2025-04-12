using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Education
{
    public class UniversityMajor : ILoreEntity
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

        public string MajorName { get; set; } = string.Empty; // Arcane Studies, Engineering, Political Science
        public string DegreeLevel { get; set; } = string.Empty; // Bachelor's, Master's, Doctorate

        //[ForeignKey("University")]
        public int UniversityId { get; set; }
        public University University { get; set; } = null!;

        public ICollection<Character> Professors { get; set; } = new List<Character>();
        //public ICollection<Character> Students { get; set; } = new List<Character>();
    }
}
