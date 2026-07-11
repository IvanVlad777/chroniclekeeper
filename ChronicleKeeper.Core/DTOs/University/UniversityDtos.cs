using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.UniversityMajor;

namespace ChronicleKeeper.Core.DTOs.University
{
    public class UniversityDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int EducationSystemId { get; set; }
        public bool FocusesOnScience { get; set; }
        public bool FocusesOnMagic { get; set; }
        public bool FocusesOnPhilosophy { get; set; }
        public bool FocusesOnMilitaryStudies { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UniversityDetailsDto : UniversityDto
    {
        public List<UniversityMajorDto> Majors { get; set; } = new();
        public List<ReferenceDto> Alumni { get; set; } = new();
    }

    public class UniversityCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>Svijet sveučilišta se izvodi iz sustava obrazovanja — ne šalje se WorldId.</summary>
        [Required]
        public int EducationSystemId { get; set; }

        public bool FocusesOnScience { get; set; }
        public bool FocusesOnMagic { get; set; }
        public bool FocusesOnPhilosophy { get; set; }
        public bool FocusesOnMilitaryStudies { get; set; }
    }

    public class UniversityUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public bool FocusesOnScience { get; set; }
        public bool FocusesOnMagic { get; set; }
        public bool FocusesOnPhilosophy { get; set; }
        public bool FocusesOnMilitaryStudies { get; set; }
    }
}
