using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.DTOs;
using static ChronicleKeeper.Core.Enums.SocietyEnums;

namespace ChronicleKeeper.Core.DTOs.Culture
{
    public class CultureDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int LanguageId { get; set; }
        public int? ReligionId { get; set; }
        public string CommonValues { get; set; } = string.Empty;
        public bool HasOralTradition { get; set; }
        public string SocialStructure { get; set; } = string.Empty;
        public XenophobiaLevel XenophobiaLevel { get; set; }
        public TechnologicalLevel TechnologicalLevel { get; set; }
        public string ConflictResolution { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CultureDetailsDto : CultureDto
    {
        public ReferenceDto? Language { get; set; }
        public ReferenceDto? Religion { get; set; }
        public List<ReferenceDto> Nations { get; set; } = new();
        public List<ReferenceDto> PracticedBySpecies { get; set; } = new();
        public List<ReferenceDto> InfluencedSocialClasses { get; set; } = new();
    }

    public class CultureCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [Required]
        public int LanguageId { get; set; }

        public int? ReligionId { get; set; }

        [StringLength(500)]
        public string CommonValues { get; set; } = string.Empty;

        public bool HasOralTradition { get; set; }

        [StringLength(500)]
        public string SocialStructure { get; set; } = string.Empty;

        public XenophobiaLevel XenophobiaLevel { get; set; }
        public TechnologicalLevel TechnologicalLevel { get; set; }

        [StringLength(500)]
        public string ConflictResolution { get; set; } = string.Empty;
    }

    public class CultureUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int LanguageId { get; set; }

        public int? ReligionId { get; set; }

        [StringLength(500)]
        public string CommonValues { get; set; } = string.Empty;

        public bool HasOralTradition { get; set; }

        [StringLength(500)]
        public string SocialStructure { get; set; } = string.Empty;

        public XenophobiaLevel XenophobiaLevel { get; set; }
        public TechnologicalLevel TechnologicalLevel { get; set; }

        [StringLength(500)]
        public string ConflictResolution { get; set; } = string.Empty;
    }
}
