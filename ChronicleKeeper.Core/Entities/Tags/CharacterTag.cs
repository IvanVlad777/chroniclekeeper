using ChronicleKeeper.Core.Entities.Characters;

namespace ChronicleKeeper.Core.Entities.Tags
{
    /// <summary>Join: Character ↔ Tag (composite PK, not a LoreEntity).</summary>
    public class CharacterTag
    {
        public int CharacterId { get; set; }
        public virtual Character? Character { get; set; }

        public int TagId { get; set; }
        public virtual Tag? Tag { get; set; }
    }
}
