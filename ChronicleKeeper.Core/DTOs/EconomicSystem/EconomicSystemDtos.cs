using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.EconomicSystem
{
    public class EconomicSystemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public bool IsMarketDriven { get; set; }
        public bool HasStateControl { get; set; }
        public bool IsFeudal { get; set; }
        public bool AllowsCorporations { get; set; }
        public bool AllowsGuilds { get; set; }
        public int? TaxationSystemId { get; set; }
        public int? BankingSystemId { get; set; }
        public int? HistoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class EconomicSystemDetailsDto : EconomicSystemDto
    {
        public ReferenceDto? TaxationSystem { get; set; }
        public ReferenceDto? BankingSystem { get; set; }
        public ReferenceDto? History { get; set; }
    }

    public class EconomicSystemCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        public bool IsMarketDriven { get; set; }
        public bool HasStateControl { get; set; }
        public bool IsFeudal { get; set; }
        public bool AllowsCorporations { get; set; }
        public bool AllowsGuilds { get; set; }

        public int? TaxationSystemId { get; set; }
        public int? BankingSystemId { get; set; }
        public int? HistoryId { get; set; }
    }

    public class EconomicSystemUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public bool IsMarketDriven { get; set; }
        public bool HasStateControl { get; set; }
        public bool IsFeudal { get; set; }
        public bool AllowsCorporations { get; set; }
        public bool AllowsGuilds { get; set; }

        public int? TaxationSystemId { get; set; }
        public int? BankingSystemId { get; set; }
        public int? HistoryId { get; set; }
    }
}
