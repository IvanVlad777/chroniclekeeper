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
    public class School : ILoreEntity
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

        public bool IsPublic { get; set; } // True = Free for all citizens
        public bool IsReligious { get; set; } // Theology-based education

        public int EducationSystemId { get; set; }
        public EducationSystem EducationSystem { get; set; } = null!;

        //public ICollection<Character> Students { get; set; } = new List<Character>();
        //public ICollection<Character> Teachers { get; set; } = new List<Character>();
        public ICollection<EducationRecord> Alumni { get; set; } = new List<EducationRecord>();
        public ICollection<SchoolSubject> Subjects { get; set; } = new List<SchoolSubject>();

        ////[ForeignKey("Country")]
        //public int? CountryId { get; set; }
        //public Country? Country { get; set; }

        ////[ForeignKey("City")]
        //public int? CityId { get; set; }
        //public City? City { get; set; }
    }
}
