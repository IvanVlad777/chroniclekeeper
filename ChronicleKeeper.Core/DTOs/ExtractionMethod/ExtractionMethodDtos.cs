using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.ExtractionMethod
{
    public class ExtractionMethodDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public string MethodType { get; set; } = string.Empty;
        public bool IsSustainable { get; set; }
        public int? HistoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ExtractionMethodDetailsDto : ExtractionMethodDto
    {
        public ReferenceDto? History { get; set; }
        public List<ReferenceDto> ResourcesExtracted { get; set; } = new();
    }

    public class ExtractionMethodCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [StringLength(100)]
        public string MethodType { get; set; } = string.Empty;

        public bool IsSustainable { get; set; }

        public int? HistoryId { get; set; }
    }

    public class ExtractionMethodUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(100)]
        public string MethodType { get; set; } = string.Empty;

        public bool IsSustainable { get; set; }

        public int? HistoryId { get; set; }
    }
}
