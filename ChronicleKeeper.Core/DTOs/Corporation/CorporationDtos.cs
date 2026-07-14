using ChronicleKeeper.Core.DTOs.CorporateLeadership;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.Corporation
{
    public class CorporationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public string IndustrySector { get; set; } = string.Empty;
        public double Revenue { get; set; }
        public int NumberOfEmployees { get; set; }
        public bool IsPubliclyTraded { get; set; }
        public bool IsStateOwned { get; set; }
        public int? IndustryId { get; set; }
        public int? TaxationSystemId { get; set; }
        public int? BankingSystemId { get; set; }
        public int? ParentCorporationId { get; set; }
        public int? HistoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CorporationDetailsDto : CorporationDto
    {
        public ReferenceDto? Industry { get; set; }
        public ReferenceDto? TaxationSystem { get; set; }
        public ReferenceDto? BankingSystem { get; set; }
        public ReferenceDto? ParentCorporation { get; set; }
        public ReferenceDto? History { get; set; }
        public List<ReferenceDto> Subsidiaries { get; set; } = new();
        public List<CorporateLeadershipDto> Leadership { get; set; } = new();
        public List<ReferenceDto> Factions { get; set; } = new();
        public List<ReferenceDto> MemberProfessions { get; set; } = new();
    }

    public class CorporationCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [StringLength(100)]
        public string IndustrySector { get; set; } = string.Empty;

        public double Revenue { get; set; }
        public int NumberOfEmployees { get; set; }
        public bool IsPubliclyTraded { get; set; }
        public bool IsStateOwned { get; set; }

        public int? IndustryId { get; set; }
        public int? TaxationSystemId { get; set; }
        public int? BankingSystemId { get; set; }
        public int? ParentCorporationId { get; set; }
        public int? HistoryId { get; set; }
    }

    public class CorporationUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(100)]
        public string IndustrySector { get; set; } = string.Empty;

        public double Revenue { get; set; }
        public int NumberOfEmployees { get; set; }
        public bool IsPubliclyTraded { get; set; }
        public bool IsStateOwned { get; set; }

        public int? IndustryId { get; set; }
        public int? TaxationSystemId { get; set; }
        public int? BankingSystemId { get; set; }
        public int? ParentCorporationId { get; set; }
        public int? HistoryId { get; set; }
    }
}
