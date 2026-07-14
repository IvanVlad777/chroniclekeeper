using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.NaturalResource
{
    public class NaturalResourceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public string ResourceType { get; set; } = string.Empty;
        public double Quantity { get; set; }
        public double MarketValue { get; set; }
        public bool IsRenewable { get; set; }
        public bool IsStrategicResource { get; set; }
        public int? ExtractionMethodId { get; set; }
        public int? HistoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class NaturalResourceDetailsDto : NaturalResourceDto
    {
        public ReferenceDto? ExtractionMethod { get; set; }
        public ReferenceDto? History { get; set; }
        public List<ReferenceDto> Locations { get; set; } = new();
        public List<ReferenceDto> ExportRoutes { get; set; } = new();
    }

    public class NaturalResourceCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [StringLength(100)]
        public string ResourceType { get; set; } = string.Empty;

        public double Quantity { get; set; }
        public double MarketValue { get; set; }
        public bool IsRenewable { get; set; }
        public bool IsStrategicResource { get; set; }

        public int? ExtractionMethodId { get; set; }
        public int? HistoryId { get; set; }
    }

    public class NaturalResourceUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(100)]
        public string ResourceType { get; set; } = string.Empty;

        public double Quantity { get; set; }
        public double MarketValue { get; set; }
        public bool IsRenewable { get; set; }
        public bool IsStrategicResource { get; set; }

        public int? ExtractionMethodId { get; set; }
        public int? HistoryId { get; set; }
    }
}
