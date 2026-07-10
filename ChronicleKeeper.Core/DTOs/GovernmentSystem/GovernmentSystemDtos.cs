using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.DTOs;
using static ChronicleKeeper.Core.Enums.SocietyEnums;

namespace ChronicleKeeper.Core.DTOs.GovernmentSystem
{
    public class GovernmentSystemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public bool IsDemocratic { get; set; }
        public bool IsMonarchic { get; set; }
        public bool IsReligious { get; set; }
        public bool IsFederal { get; set; }
        public bool IsCentralized { get; set; }
        public int? PoliticalIdeologyId { get; set; }
        public ElectionSystem ElectionSystem { get; set; }
        public ScaleLevel StabilityLevel { get; set; }
        public bool HasTermLimits { get; set; }
        public int? MaxTermLength { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class GovernmentSystemDetailsDto : GovernmentSystemDto
    {
        public ReferenceDto? PoliticalIdeology { get; set; }
        public List<ReferenceDto> PoliticalParties { get; set; } = new();
    }

    public class GovernmentSystemCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        public bool IsDemocratic { get; set; }
        public bool IsMonarchic { get; set; }
        public bool IsReligious { get; set; }
        public bool IsFederal { get; set; }
        public bool IsCentralized { get; set; }
        public int? PoliticalIdeologyId { get; set; }

        public ElectionSystem ElectionSystem { get; set; }
        public ScaleLevel StabilityLevel { get; set; }

        public bool HasTermLimits { get; set; }
        public int? MaxTermLength { get; set; }
    }

    public class GovernmentSystemUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public bool IsDemocratic { get; set; }
        public bool IsMonarchic { get; set; }
        public bool IsReligious { get; set; }
        public bool IsFederal { get; set; }
        public bool IsCentralized { get; set; }
        public int? PoliticalIdeologyId { get; set; }

        public ElectionSystem ElectionSystem { get; set; }
        public ScaleLevel StabilityLevel { get; set; }

        public bool HasTermLimits { get; set; }
        public int? MaxTermLength { get; set; }
    }
}
