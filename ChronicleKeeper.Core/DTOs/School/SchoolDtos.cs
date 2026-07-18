using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.SchoolSubject;

namespace ChronicleKeeper.Core.DTOs.School
{
    public class SchoolDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int EducationSystemId { get; set; }
        public bool IsPublic { get; set; }
        public bool IsReligious { get; set; }
        public int? LocationId { get; set; }

        /// <summary>Read-only TPH discriminator: "School" or "TradeSchool".</summary>
        public string SchoolType { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class SchoolDetailsDto : SchoolDto
    {
        public List<SchoolSubjectDto> Subjects { get; set; } = new();
        public List<ReferenceDto> Alumni { get; set; } = new();
        public ReferenceDto? Location { get; set; }
        public List<ReferenceDto> Students { get; set; } = new();
        public List<ReferenceDto> Teachers { get; set; } = new();
    }

    public class SchoolCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>Svijet škole se izvodi iz sustava obrazovanja — ne šalje se WorldId.</summary>
        [Required]
        public int EducationSystemId { get; set; }

        public bool IsPublic { get; set; }
        public bool IsReligious { get; set; }
        public int? LocationId { get; set; }
    }

    // Sustav obrazovanja škole se ne mijenja nakon stvaranja — zato bez EducationSystemId
    public class SchoolUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public bool IsPublic { get; set; }
        public bool IsReligious { get; set; }
        public int? LocationId { get; set; }
    }
}
