using ChronicleKeeper.Core.Entities.Geography;

namespace ChronicleKeeper.Core.Entities.Geography.Creatures
{
    /// <summary>Join: Creature ↔ City (composite PK, not a LoreEntity).</summary>
    public class CreatureCity
    {
        public int CreatureId { get; set; }
        public virtual Creature? Creature { get; set; }

        public int CityId { get; set; }
        public virtual City? City { get; set; }
    }
}
