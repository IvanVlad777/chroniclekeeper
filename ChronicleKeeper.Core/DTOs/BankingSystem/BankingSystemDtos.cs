using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.BankingSystem
{
    public class BankingSystemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public string SystemType { get; set; } = string.Empty;
        public double InterestRate { get; set; }
        public bool AllowsLoans { get; set; }
        public bool HasStateControl { get; set; }
        public bool SupportsForeignInvestment { get; set; }
        public int? CurrencyId { get; set; }
        public int? HistoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class BankingSystemDetailsDto : BankingSystemDto
    {
        public ReferenceDto? Currency { get; set; }
        public ReferenceDto? History { get; set; }
    }

    public class BankingSystemCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [StringLength(100)]
        public string SystemType { get; set; } = string.Empty;

        public double InterestRate { get; set; }
        public bool AllowsLoans { get; set; }
        public bool HasStateControl { get; set; }
        public bool SupportsForeignInvestment { get; set; }

        public int? CurrencyId { get; set; }
        public int? HistoryId { get; set; }
    }

    public class BankingSystemUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(100)]
        public string SystemType { get; set; } = string.Empty;

        public double InterestRate { get; set; }
        public bool AllowsLoans { get; set; }
        public bool HasStateControl { get; set; }
        public bool SupportsForeignInvestment { get; set; }

        public int? CurrencyId { get; set; }
        public int? HistoryId { get; set; }
    }
}
