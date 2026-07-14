using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.TaxationSystem
{
    public class TaxationSystemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public double IncomeTaxRate { get; set; }
        public double CorporateTaxRate { get; set; }
        public double TradeTariffRate { get; set; }
        public bool HasFlatTax { get; set; }
        public bool HasWealthTax { get; set; }
        public int? HistoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class TaxationSystemDetailsDto : TaxationSystemDto
    {
        public ReferenceDto? History { get; set; }
    }

    public class TaxationSystemCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [Range(0, 100)]
        public double IncomeTaxRate { get; set; }
        [Range(0, 100)]
        public double CorporateTaxRate { get; set; }
        [Range(0, 100)]
        public double TradeTariffRate { get; set; }

        public bool HasFlatTax { get; set; }
        public bool HasWealthTax { get; set; }

        public int? HistoryId { get; set; }
    }

    public class TaxationSystemUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Range(0, 100)]
        public double IncomeTaxRate { get; set; }
        [Range(0, 100)]
        public double CorporateTaxRate { get; set; }
        [Range(0, 100)]
        public double TradeTariffRate { get; set; }

        public bool HasFlatTax { get; set; }
        public bool HasWealthTax { get; set; }

        public int? HistoryId { get; set; }
    }
}
