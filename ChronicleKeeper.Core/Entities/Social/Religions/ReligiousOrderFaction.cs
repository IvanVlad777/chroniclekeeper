using ChronicleKeeper.Core.Entities.Social.Politics;

namespace ChronicleKeeper.Core.Entities.Social.Religions
{
    /// <summary>Join: ReligiousOrder ↔ Faction. Composite PK, not a LoreEntity. Faction is the asymmetric target.</summary>
    public class ReligiousOrderFaction
    {
        public int ReligiousOrderId { get; set; }
        public virtual ReligiousOrder? ReligiousOrder { get; set; }

        public int FactionId { get; set; }
        public virtual Faction? Faction { get; set; }
    }
}
