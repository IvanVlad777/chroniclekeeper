using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.AbilityLevel
{
    public class AbilityLevelDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int AbilityId { get; set; }
        public string Rank { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class AbilityLevelCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>Svijet razine se izvodi iz sposobnosti — ne šalje se WorldId.</summary>
        [Required]
        public int AbilityId { get; set; }

        [Required]
        public string Rank { get; set; } = string.Empty;
    }

    public class AbilityLevelUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Rank { get; set; } = string.Empty;
    }
}
