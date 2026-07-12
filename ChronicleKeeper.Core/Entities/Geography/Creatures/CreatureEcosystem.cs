using ChronicleKeeper.Core.Entities.Geography.Ecosystems;

namespace ChronicleKeeper.Core.Entities.Geography.Creatures
{
    /// <summary>Join: Creature ↔ Ecosystem (composite PK, not a LoreEntity). Mirrors CreatureCity —
    /// Creature owns the collection (Habitants), Ecosystem has no reverse nav.</summary>
    public class CreatureEcosystem
    {
        public int CreatureId { get; set; }
        public virtual Creature? Creature { get; set; }

        public int EcosystemId { get; set; }
        public virtual Ecosystem? Ecosystem { get; set; }
    }
}
