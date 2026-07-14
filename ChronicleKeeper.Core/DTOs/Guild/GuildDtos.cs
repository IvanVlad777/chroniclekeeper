using ChronicleKeeper.Core.DTOs.GuildRank;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.Guild
{
    public class GuildDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public string GuildType { get; set; } = string.Empty;
        public string PrimaryActivity { get; set; } = string.Empty;
        public bool IsGovernmentSanctioned { get; set; }
        public int? TaxationSystemId { get; set; }
        public int? IndustryId { get; set; }
        public int? LegalSystemId { get; set; }
        public int? EducationSystemId { get; set; }
        public int? HistoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class GuildDetailsDto : GuildDto
    {
        public ReferenceDto? TaxationSystem { get; set; }
        public ReferenceDto? Industry { get; set; }
        public ReferenceDto? LegalSystem { get; set; }
        public ReferenceDto? EducationSystem { get; set; }
        public ReferenceDto? History { get; set; }
        public List<GuildRankDto> GuildRanks { get; set; } = new();
        public List<ReferenceDto> Factions { get; set; } = new();
        public List<ReferenceDto> MemberProfessions { get; set; } = new();
        public List<ReferenceDto> SocialClasses { get; set; } = new();
    }

    public class GuildCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [StringLength(100)]
        public string GuildType { get; set; } = string.Empty;

        [StringLength(200)]
        public string PrimaryActivity { get; set; } = string.Empty;

        public bool IsGovernmentSanctioned { get; set; }

        public int? TaxationSystemId { get; set; }
        public int? IndustryId { get; set; }
        public int? LegalSystemId { get; set; }
        public int? EducationSystemId { get; set; }
        public int? HistoryId { get; set; }
    }

    public class GuildUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(100)]
        public string GuildType { get; set; } = string.Empty;

        [StringLength(200)]
        public string PrimaryActivity { get; set; } = string.Empty;

        public bool IsGovernmentSanctioned { get; set; }

        public int? TaxationSystemId { get; set; }
        public int? IndustryId { get; set; }
        public int? LegalSystemId { get; set; }
        public int? EducationSystemId { get; set; }
        public int? HistoryId { get; set; }
    }
}
