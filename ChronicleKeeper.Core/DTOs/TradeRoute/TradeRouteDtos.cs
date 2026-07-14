using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.TradeRoute
{
    public class TradeRouteDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public string RouteType { get; set; } = string.Empty;
        public string MainGoods { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class TradeRouteDetailsDto : TradeRouteDto
    {
        public ReferenceDto? History { get; set; }
        public List<ReferenceDto> Locations { get; set; } = new();
        public List<ReferenceDto> ResourcesTraded { get; set; } = new();
    }

    public class TradeRouteCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [StringLength(50)]
        public string RouteType { get; set; } = string.Empty;

        [StringLength(200)]
        public string MainGoods { get; set; } = string.Empty;

        public int? HistoryId { get; set; }
    }

    public class TradeRouteUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(50)]
        public string RouteType { get; set; } = string.Empty;

        [StringLength(200)]
        public string MainGoods { get; set; } = string.Empty;

        public int? HistoryId { get; set; }
    }
}
