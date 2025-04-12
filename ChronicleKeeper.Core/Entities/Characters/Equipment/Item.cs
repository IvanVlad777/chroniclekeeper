using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social;
using ChronicleKeeper.Core.Entities.Social.Structure;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ChronicleKeeper.Core.Enums.SocietyEnums;

namespace ChronicleKeeper.Core.Entities.Characters.Equipment
{
    public class Item : ILoreEntity
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

        public string Category { get; set; } = string.Empty; // Weapon, Armor, Tool, Magical Item
        public FactionType FactionType { get; set; } // ✅ Uses enum instead of a string
        public bool IsUnique { get; set; } // True = Legendary or one-of-a-kind
        public string Material { get; set; } = string.Empty; // Steel, Leather, Dragonbone
        public string SpecialProperties { get; set; } = string.Empty; // "Glows in darkness", "+10 Fire Resistance"
        public string Rarity { get; set; } = "Common"; // Common, Uncommon, Rare, Legendary, Mythical

        //[ForeignKey("CurrentOwner")]
        public int? CurrentOwnerId { get; set; } // CurrentOwner
        public Character? CurrentOwner { get; set; }


        //[ForeignKey("StoredAt")] // Gdje je trenutno predmet (npr. u riznici, hramu) StoredAt
        public int? StoredAtId { get; set; }
        public Location? StoredAt { get; set; }


        //[ForeignKey("Faction")]
        public int? FactionId { get; set; }
        public Faction? Faction { get; set; }


        // ✅ Povijest vlasništva
        //public ICollection<OwnershipHistory> OwnershipHistory { get; set; } = new List<OwnershipHistory>();
    }

}
