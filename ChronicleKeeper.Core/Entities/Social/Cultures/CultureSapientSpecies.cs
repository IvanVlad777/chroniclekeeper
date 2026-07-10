using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    /// <summary>Join: Culture ↔ SapientSpecies (composite PK, not a LoreEntity).</summary>
    public class CultureSapientSpecies
    {
        public int CultureId { get; set; }
        public virtual Culture? Culture { get; set; }

        public int SapientSpeciesId { get; set; }
        public virtual SapientSpecies? SapientSpecies { get; set; }
    }
}
