using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.DTOs;

namespace ChronicleKeeper.Core.DTOs.SocialHierarchy
{
    public class SocialHierarchyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public bool IsCasteSystem { get; set; }
        public bool AllowsUpwardMobility { get; set; }
        public bool AllowsIntermarriage { get; set; }
        public bool EnforcesLegalSeparation { get; set; }
        public int? HistoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class SocialHierarchyDetailsDto : SocialHierarchyDto
    {
        public ReferenceDto? History { get; set; }
        public List<ReferenceDto> Classes { get; set; } = new();
        public List<ReferenceDto> Nations { get; set; } = new();
    }

    public class SocialHierarchyCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        public bool IsCasteSystem { get; set; }
        public bool AllowsUpwardMobility { get; set; }
        public bool AllowsIntermarriage { get; set; }
        public bool EnforcesLegalSeparation { get; set; }
        public int? HistoryId { get; set; }
    }

    public class SocialHierarchyUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public bool IsCasteSystem { get; set; }
        public bool AllowsUpwardMobility { get; set; }
        public bool AllowsIntermarriage { get; set; }
        public bool EnforcesLegalSeparation { get; set; }
        public int? HistoryId { get; set; }
    }
}
