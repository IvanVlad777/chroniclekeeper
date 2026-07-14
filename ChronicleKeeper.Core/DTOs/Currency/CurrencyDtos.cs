using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.Currency
{
    public class CurrencyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public double ExchangeRate { get; set; }
        public string BackingType { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CurrencyDetailsDto : CurrencyDto
    {
        public ReferenceDto? History { get; set; }
    }

    public class CurrencyCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [StringLength(10)]
        public string Symbol { get; set; } = string.Empty;

        public double ExchangeRate { get; set; }

        [StringLength(100)]
        public string BackingType { get; set; } = string.Empty;

        public int? HistoryId { get; set; }
    }

    public class CurrencyUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(10)]
        public string Symbol { get; set; } = string.Empty;

        public double ExchangeRate { get; set; }

        [StringLength(100)]
        public string BackingType { get; set; } = string.Empty;

        public int? HistoryId { get; set; }
    }
}
