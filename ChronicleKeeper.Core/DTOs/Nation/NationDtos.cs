using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.DTOs;

namespace ChronicleKeeper.Core.DTOs.Nation
{
    public class NationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int Population { get; set; }
        public int? HistoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class NationDetailsDto : NationDto
    {
        public List<ReferenceDto> Citizens { get; set; } = new();
        public ReferenceDto? History { get; set; }
    }

    public class NationCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Population cannot be negative")]
        public int Population { get; set; }
        public int? HistoryId { get; set; }
    }

    public class NationUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Population cannot be negative")]
        public int Population { get; set; }
        public int? HistoryId { get; set; }
    }
}
