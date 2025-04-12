using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class Clothing : ILoreEntity
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

        public string ClothingType { get; set; } = string.Empty; // Robe, Tunic, Armor, Dress
        public string Materials { get; set; } = string.Empty; // Cotton, Silk, Leather, Metal
        public string DesignFeatures { get; set; } = string.Empty; // Embroidery, Gems, Fur-lined
        public bool IsRitualistic { get; set; } // Used for ceremonies?
        public bool IsArmor { get; set; } // Is this a protective clothing piece
        public string SpecialProperties { get; set; } = string.Empty; // Fire-resistant, Enchanted, Lightweight

        //[ForeignKey("Culture")]
        public int CultureId { get; set; }
        public Culture Culture { get; set; } = null!;

        public ICollection<Character> Character { get; set; } = new List<Character>(); // ✅ Who specializes in this?

    }
}