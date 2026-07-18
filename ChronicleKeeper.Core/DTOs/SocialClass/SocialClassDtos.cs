using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.DTOs;

namespace ChronicleKeeper.Core.DTOs.SocialClass
{
    public class SocialClassDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public bool IsNoble { get; set; }
        public bool IsMerchantClass { get; set; }
        public bool IsOutcast { get; set; }
        public bool CanOwnLand { get; set; }
        public bool CanHoldOffice { get; set; }
        public bool HasTaxExemptions { get; set; }
        public int? SocialHierarchyId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class SocialClassDetailsDto : SocialClassDto
    {
        public List<ReferenceDto> Members { get; set; } = new();
    }

    public class SocialClassCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        public bool IsNoble { get; set; }
        public bool IsMerchantClass { get; set; }
        public bool IsOutcast { get; set; }
        public bool CanOwnLand { get; set; }
        public bool CanHoldOffice { get; set; }
        public bool HasTaxExemptions { get; set; }
        public int? SocialHierarchyId { get; set; }
    }

    public class SocialClassUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public bool IsNoble { get; set; }
        public bool IsMerchantClass { get; set; }
        public bool IsOutcast { get; set; }
        public bool CanOwnLand { get; set; }
        public bool CanHoldOffice { get; set; }
        public bool HasTaxExemptions { get; set; }
        public int? SocialHierarchyId { get; set; }
    }
}
