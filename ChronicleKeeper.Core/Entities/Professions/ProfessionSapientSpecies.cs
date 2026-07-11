using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;

namespace ChronicleKeeper.Core.Entities.Professions
{
    /// <summary>Join: Profession ↔ SapientSpecies (composite PK, not a LoreEntity).</summary>
    public class ProfessionSapientSpecies
    {
        public int ProfessionId { get; set; }
        public virtual Profession? Profession { get; set; }

        public int SapientSpeciesId { get; set; }
        public virtual SapientSpecies? SapientSpecies { get; set; }
    }
}
