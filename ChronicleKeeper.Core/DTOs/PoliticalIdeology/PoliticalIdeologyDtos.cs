using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.DTOs;

namespace ChronicleKeeper.Core.DTOs.PoliticalIdeology
{
    public class PoliticalIdeologyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public bool IsAuthoritarian { get; set; }
        public bool IsSocialist { get; set; }
        public bool IsLiberal { get; set; }
        public bool IsRadical { get; set; }
        public bool IsMilitaristic { get; set; }
        public bool SupportsFreeMarket { get; set; }
        public bool SupportsPlannedEconomy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class PoliticalIdeologyDetailsDto : PoliticalIdeologyDto
    {
        public List<ReferenceDto> AffiliatedPoliticalParties { get; set; } = new();
        public List<ReferenceDto> AffiliatedGovernmentSystems { get; set; } = new();
    }

    public class PoliticalIdeologyCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        public bool IsAuthoritarian { get; set; }
        public bool IsSocialist { get; set; }
        public bool IsLiberal { get; set; }
        public bool IsRadical { get; set; }
        public bool IsMilitaristic { get; set; }
        public bool SupportsFreeMarket { get; set; }
        public bool SupportsPlannedEconomy { get; set; }
    }

    public class PoliticalIdeologyUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public bool IsAuthoritarian { get; set; }
        public bool IsSocialist { get; set; }
        public bool IsLiberal { get; set; }
        public bool IsRadical { get; set; }
        public bool IsMilitaristic { get; set; }
        public bool SupportsFreeMarket { get; set; }
        public bool SupportsPlannedEconomy { get; set; }
    }
}
