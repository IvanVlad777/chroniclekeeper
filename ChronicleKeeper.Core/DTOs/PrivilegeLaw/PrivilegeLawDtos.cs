using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.PrivilegeLaw
{
    public class PrivilegeLawDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int SocialClassId { get; set; }
        public bool GrantsLegalImmunity { get; set; }
        public bool GrantsLandOwnershipRights { get; set; }
        public bool AllowsPrivateArmies { get; set; }
        public int? HistoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class PrivilegeLawCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>The law's world is derived from its social class — no WorldId is sent.</summary>
        [Required]
        public int SocialClassId { get; set; }

        public bool GrantsLegalImmunity { get; set; }
        public bool GrantsLandOwnershipRights { get; set; }
        public bool AllowsPrivateArmies { get; set; }
        public int? HistoryId { get; set; }
    }

    public class PrivilegeLawUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public bool GrantsLegalImmunity { get; set; }
        public bool GrantsLandOwnershipRights { get; set; }
        public bool AllowsPrivateArmies { get; set; }
        public int? HistoryId { get; set; }
    }
}
