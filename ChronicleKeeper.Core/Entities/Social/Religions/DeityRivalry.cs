using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;

namespace ChronicleKeeper.Core.Entities.Social.Religions
{
    /// <summary>Self-referencing join: Deity ↔ Deity (rivals). Composite PK, not a LoreEntity.
    /// DeityId cascades; RivalDeityId is Restrict (two FKs to the same table — only one may cascade).</summary>
    public class DeityRivalry
    {
        public int DeityId { get; set; }
        public virtual Deity? Deity { get; set; }

        public int RivalDeityId { get; set; }
        public virtual Deity? RivalDeity { get; set; }
    }
}
