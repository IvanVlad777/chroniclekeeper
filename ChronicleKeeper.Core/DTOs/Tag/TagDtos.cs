using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.Tag
{
    public class TagDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class TagCreateDto
    {
        [Required]
        [StringLength(50, ErrorMessage = "Tag name cannot exceed 50 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }
    }

    public class TagUpdateDto
    {
        [Required]
        [StringLength(50, ErrorMessage = "Tag name cannot exceed 50 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>Na koji tip entiteta se tag veže.</summary>
    public enum TagTargetType
    {
        Character,
        Location,
        Faction
    }

    public class TagAttachDto
    {
        [Required]
        public TagTargetType TargetType { get; set; }

        [Required]
        public int TargetId { get; set; }
    }
}
