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

        //public virtual ICollection<Characters.Character> Teachers { get; set; } = new List<Characters.Character>(); // TODO: Uncomment when Character many-to-many cross-links are revived
    }
}
