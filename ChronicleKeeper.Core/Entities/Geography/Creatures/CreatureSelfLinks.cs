namespace ChronicleKeeper.Core.Entities.Geography.Creatures
{
    /// <summary>Self-referencing join: Creature ↔ Creature (symbiotic partners). Composite PK, not a
    /// LoreEntity. CreatureId cascades; SymbioticPartnerId is Restrict (two FKs to the same table —
    /// only one may cascade). Stored one-directional, mirrors DeityAlliance.</summary>
    public class CreatureSymbiosis
    {
        public int CreatureId { get; set; }
        public virtual Creature? Creature { get; set; }

        public int SymbioticPartnerId { get; set; }
        public virtual Creature? SymbioticPartner { get; set; }
    }

    /// <summary>Self-referencing join: Creature ↔ Creature (predation). Composite PK, not a LoreEntity.
    /// The predator side (PredatorCreatureId) cascades; the prey side (PreyCreatureId) is Restrict.
    /// A creature's Prey = predations where it is the predator; its Predators = predations where it is
    /// the prey.</summary>
    public class CreaturePredation
    {
        public int PredatorCreatureId { get; set; }
        public virtual Creature? Predator { get; set; }

        public int PreyCreatureId { get; set; }
        public virtual Creature? Prey { get; set; }
    }
}
