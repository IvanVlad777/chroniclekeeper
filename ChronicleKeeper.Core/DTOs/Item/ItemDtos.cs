using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.Item
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public string Category { get; set; } = string.Empty;
        public bool IsUnique { get; set; }
        public string Material { get; set; } = string.Empty;
        public string SpecialProperties { get; set; } = string.Empty;
        public string Rarity { get; set; } = string.Empty;
        public int? CurrentOwnerId { get; set; }
        public int? StoredAtId { get; set; }
        public int? FactionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ItemCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

        public bool IsUnique { get; set; }

        [StringLength(100)]
        public string Material { get; set; } = string.Empty;

        [StringLength(500)]
        public string SpecialProperties { get; set; } = string.Empty;

        public string Rarity { get; set; } = "Common";

        public int? CurrentOwnerId { get; set; }
        public int? StoredAtId { get; set; }
        public int? FactionId { get; set; }
    }

    public class ItemUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        public bool IsUnique { get; set; }

        [StringLength(100)]
        public string Material { get; set; } = string.Empty;

        [StringLength(500)]
        public string SpecialProperties { get; set; } = string.Empty;

        public string Rarity { get; set; } = "Common";

        public int? CurrentOwnerId { get; set; }
        public int? StoredAtId { get; set; }
        public int? FactionId { get; set; }
    }
}
