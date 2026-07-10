using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.DTOs;
using static ChronicleKeeper.Core.Enums.SocietyEnums;

namespace ChronicleKeeper.Core.DTOs.PoliticalParty
{
    public class PoliticalPartyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public string IdeologyDescription { get; set; } = string.Empty;
        public int PoliticalIdeologyId { get; set; }
        public int? GovernmentSystemId { get; set; }
        public ScaleLevel InfluenceLevel { get; set; }
        public bool IsBanned { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class PoliticalPartyDetailsDto : PoliticalPartyDto
    {
        public ReferenceDto? PoliticalIdeology { get; set; }
        public ReferenceDto? GovernmentSystem { get; set; }
    }

    public class PoliticalPartyCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [StringLength(2000, ErrorMessage = "Ideology description cannot exceed 2000 characters")]
        public string IdeologyDescription { get; set; } = string.Empty;

        [Required]
        public int PoliticalIdeologyId { get; set; }

        public int? GovernmentSystemId { get; set; }

        public ScaleLevel InfluenceLevel { get; set; }
        public bool IsBanned { get; set; }
    }

    public class PoliticalPartyUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "Ideology description cannot exceed 2000 characters")]
        public string IdeologyDescription { get; set; } = string.Empty;

        [Required]
        public int PoliticalIdeologyId { get; set; }

        public int? GovernmentSystemId { get; set; }

        public ScaleLevel InfluenceLevel { get; set; }
        public bool IsBanned { get; set; }
    }
}
