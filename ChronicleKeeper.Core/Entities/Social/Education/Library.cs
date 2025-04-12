using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Education
{
    public class Library : ILoreEntity
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

        public bool IsPublic { get; set; } // Open to all citizens
        public bool FocusesOnMagic { get; set; } // Specializes in magical texts
        public bool FocusesOnHistory { get; set; } // Archives and historical records

        //public ICollection<Character> Scholars { get; set; } = new List<Character>();

        //[ForeignKey("University")]
        public int? UniversityId { get; set; }
        public University? University { get; set; }

        //[ForeignKey("Location")] //Možda treba biti i na Location
        public int? LocationId { get; set; }
        public Location? Location { get; set; }

    }
}
