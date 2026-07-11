using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.OwnershipHistory
{
    public class OwnershipHistoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int ItemId { get; set; }
        public int? PreviousOwnerId { get; set; }
        public int? NewOwnerId { get; set; }
        public string DateAcquired { get; set; } = string.Empty;
        public string TransferReason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class OwnershipHistoryCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>Svijet zapisa se izvodi iz predmeta — ne šalje se WorldId.</summary>
        [Required]
        public int ItemId { get; set; }

        public int? PreviousOwnerId { get; set; }
        public int? NewOwnerId { get; set; }

        [StringLength(100)]
        public string DateAcquired { get; set; } = string.Empty;

        [Required]
        public string TransferReason { get; set; } = string.Empty;
    }

    public class OwnershipHistoryUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public int? PreviousOwnerId { get; set; }
        public int? NewOwnerId { get; set; }

        [StringLength(100)]
        public string DateAcquired { get; set; } = string.Empty;

        [Required]
        public string TransferReason { get; set; } = string.Empty;
    }
}
