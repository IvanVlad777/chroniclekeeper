using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.SchoolSubject;

namespace ChronicleKeeper.Core.DTOs.TradeSchool
{
    public class TradeSchoolDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int EducationSystemId { get; set; }
        public bool IsPublic { get; set; }
        public bool IsReligious { get; set; }
        public string Specialization { get; set; } = string.Empty;
        public int DurationYears { get; set; }
        public bool IsGovernmentRecognized { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class TradeSchoolDetailsDto : TradeSchoolDto
    {
        public List<SchoolSubjectDto> Subjects { get; set; } = new();
        public List<ReferenceDto> Alumni { get; set; } = new();
        public List<ReferenceDto> TrainedProfessions { get; set; } = new();
        public List<ReferenceDto> Apprenticeships { get; set; } = new();
    }

    public class TradeSchoolCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>Svijet strukovne škole se izvodi iz sustava obrazovanja — ne šalje se WorldId.</summary>
        [Required]
        public int EducationSystemId { get; set; }

        public bool IsPublic { get; set; }
        public bool IsReligious { get; set; }

        [StringLength(100)]
        public string Specialization { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "DurationYears cannot be negative")]
        public int DurationYears { get; set; }

        public bool IsGovernmentRecognized { get; set; }
    }

    // Sustav obrazovanja strukovne škole se ne mijenja nakon stvaranja — zato bez EducationSystemId
    public class TradeSchoolUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public bool IsPublic { get; set; }
        public bool IsReligious { get; set; }

        [StringLength(100)]
        public string Specialization { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "DurationYears cannot be negative")]
        public int DurationYears { get; set; }

        public bool IsGovernmentRecognized { get; set; }
    }
}
