using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.History
{
    public class HistoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public string Summary { get; set; } = string.Empty;
        public bool IsOfficial { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class HistoryDetailsDto : HistoryDto
    {
        public List<ReferenceDto> Timelines { get; set; } = new();
    }

    public class HistoryCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [StringLength(4000, ErrorMessage = "Summary cannot exceed 4000 characters")]
        public string Summary { get; set; } = string.Empty;

        public bool IsOfficial { get; set; }
    }

    public class HistoryUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Summary cannot exceed 4000 characters")]
        public string Summary { get; set; } = string.Empty;

        public bool IsOfficial { get; set; }
    }
}
