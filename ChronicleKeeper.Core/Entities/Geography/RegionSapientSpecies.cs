using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;

namespace ChronicleKeeper.Core.Entities.Geography
{
    /// <summary>Join: Region ↔ SapientSpecies (composite PK, not a LoreEntity).</summary>
    public class RegionSapientSpecies
    {
        public int RegionId { get; set; }
        public virtual Region? Region { get; set; }

        public int SapientSpeciesId { get; set; }
        public virtual SapientSpecies? SapientSpecies { get; set; }
    }
}
