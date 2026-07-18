using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;

namespace ChronicleKeeper.Core.Entities.Social.Religions
{
    /// <summary>Join: Deity ↔ ReligiousOrder (orders dedicated to a deity). Composite PK, not a LoreEntity.</summary>
    public class DeityReligiousOrder
    {
        public int DeityId { get; set; }
        public virtual Deity? Deity { get; set; }

        public int ReligiousOrderId { get; set; }
        public virtual ReligiousOrder? ReligiousOrder { get; set; }
    }
}
