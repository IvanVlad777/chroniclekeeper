using ChronicleKeeper.Core.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ChronicleKeeper.Core.Enums.EquipmentEnums;

namespace ChronicleKeeper.Core.Entities.Characters.Equipment
{
    public class OwnershipHistory : LoreEntity
    {
        //public virtual History? History { get; set; } // TODO: Uncomment when History entity is revived

        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public virtual Item? Item { get; set; }

        [ForeignKey("PreviousOwner")]
        public int? PreviousOwnerId { get; set; }
        public virtual Character? PreviousOwner { get; set; }

        [ForeignKey("NewOwner")]
        public int? NewOwnerId { get; set; }
        public virtual Character? NewOwner { get; set; }

        public string DateAcquired { get; set; } = string.Empty; // Fikcijski datum, stopgap dok ne postoji vlastiti kalendarski sustav

        [Required]
        public OwnershipTransferReason TransferReason { get; set; }
    }
}
