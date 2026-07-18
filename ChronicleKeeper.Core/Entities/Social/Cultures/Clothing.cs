using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class Clothing : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public string ClothingType { get; set; } = string.Empty; // Robe, Tunic, Armor, Dress
        public string Materials { get; set; } = string.Empty; // Cotton, Silk, Leather, Metal
        public string DesignFeatures { get; set; } = string.Empty; // Embroidery, Gems, Fur-lined
        public bool IsRitualistic { get; set; } // Used for ceremonies?
        public bool IsArmor { get; set; } // Is this a protective clothing piece
        public string SpecialProperties { get; set; } = string.Empty; // Fire-resistant, Enchanted, Lightweight

        public int CultureId { get; set; }
        public virtual Culture? Culture { get; set; }

        public virtual ICollection<CharacterClothing> Wearers { get; set; } = new List<CharacterClothing>();
    }
}
