using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Characters.Equipment
{
    public class OwnershipHistory : ILoreEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public virtual History? History { get; set; }

        //[ForeignKey("Item")]
        //public int ItemId { get; set; }
        //public Item? Item { get; set; }

        //[ForeignKey("PreviousOwner")]
        //public int? PreviousOwnerId { get; set; }
        //public Character? PreviousOwner { get; set; }

        //[ForeignKey("NewOwner")]
        //public int? NewOwnerId { get; set; }
        //public Character? NewOwner { get; set; }


        //public DateTime DateAcquired { get; set; } = DateTime.UtcNow;
        //public string TransferReason { get; set; } = string.Empty; // Stolen, Inherited, Gifted, Lost, Found
    }
}