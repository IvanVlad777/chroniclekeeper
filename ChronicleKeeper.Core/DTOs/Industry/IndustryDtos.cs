using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.Industry
{
    public class IndustryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public string Sector { get; set; } = string.Empty;
        public double EmploymentRate { get; set; }
        public int? HistoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class IndustryDetailsDto : IndustryDto
    {
        public ReferenceDto? History { get; set; }
    }

    public class IndustryCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [StringLength(100)]
        public string Sector { get; set; } = string.Empty;

        [Range(0, 100)]
        public double EmploymentRate { get; set; }

        public int? HistoryId { get; set; }
    }

    public class IndustryUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(100)]
        public string Sector { get; set; } = string.Empty;

        [Range(0, 100)]
        public double EmploymentRate { get; set; }

        public int? HistoryId { get; set; }
    }
}
