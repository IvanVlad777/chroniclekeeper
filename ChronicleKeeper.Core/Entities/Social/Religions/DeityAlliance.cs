using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;

namespace ChronicleKeeper.Core.Entities.Social.Religions
{
    /// <summary>Self-referencing join: Deity ↔ Deity (allies). Composite PK, not a LoreEntity.
    /// DeityId cascades; AlliedDeityId is Restrict (two FKs to the same table — only one may cascade).</summary>
    public class DeityAlliance
    {
        public int DeityId { get; set; }
        public virtual Deity? Deity { get; set; }

        public int AlliedDeityId { get; set; }
        public virtual Deity? AlliedDeity { get; set; }
    }
}
