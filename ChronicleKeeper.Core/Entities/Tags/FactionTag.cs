using ChronicleKeeper.Core.Entities.Social;

namespace ChronicleKeeper.Core.Entities.Tags
{
    /// <summary>Join: Faction ↔ Tag (composite PK, not a LoreEntity).</summary>
    public class FactionTag
    {
        public int FactionId { get; set; }
        public virtual Faction? Faction { get; set; }

        public int TagId { get; set; }
        public virtual Tag? Tag { get; set; }
    }
}
