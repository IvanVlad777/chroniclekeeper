using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.EducationRecord
{
    public class EducationRecordDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? CharacterId { get; set; }
        public int? SchoolId { get; set; }
        public int? UniversityId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Degree { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class EducationRecordCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        public int? CharacterId { get; set; }
        public int? SchoolId { get; set; }
        public int? UniversityId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(100)]
        public string Degree { get; set; } = string.Empty;
    }

    public class EducationRecordUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public int? CharacterId { get; set; }
        public int? SchoolId { get; set; }
        public int? UniversityId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(100)]
        public string Degree { get; set; } = string.Empty;
    }
}
