using ChronicleKeeper.Core.Entities.Characters;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    /// <summary>Join: Character ↔ Clothing (composite PK, not a LoreEntity).</summary>
    public class CharacterClothing
    {
        public int CharacterId { get; set; }
        public virtual Character? Character { get; set; }

        public int ClothingId { get; set; }
        public virtual Clothing? Clothing { get; set; }
    }
}
