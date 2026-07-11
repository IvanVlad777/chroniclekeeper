using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.Social;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ChronicleKeeper.Core.Enums.EquipmentEnums;

namespace ChronicleKeeper.Core.Entities.Characters.Equipment
{
    public class Item : LoreEntity
    {
        //public virtual History? History { get; set; } // TODO: Uncomment when History entity is revived

        [Required]
        public ItemCategory Category { get; set; }
        public bool IsUnique { get; set; } // True = Legendary or one-of-a-kind
        public string Material { get; set; } = string.Empty; // Steel, Leather, Dragonbone
        public string SpecialProperties { get; set; } = string.Empty; // "Glows in darkness", "+10 Fire Resistance"
        [Required]
        public ItemRarity Rarity { get; set; } = ItemRarity.Common;

        [ForeignKey("CurrentOwner")]
        public int? CurrentOwnerId { get; set; }
        public virtual Character? CurrentOwner { get; set; }

        [ForeignKey("StoredAt")] // Gdje je trenutno predmet (npr. u riznici, hramu)
        public int? StoredAtId { get; set; }
        public virtual Location? StoredAt { get; set; }

        [ForeignKey("Faction")]
        public int? FactionId { get; set; }
        public virtual Faction? Faction { get; set; }

        public virtual ICollection<OwnershipHistory> OwnershipHistory { get; set; } = new List<OwnershipHistory>();
    }
}
