using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    /// <summary>Join: Folklore ↔ SapientSpecies (species this folklore originated from). Composite PK, not a LoreEntity.</summary>
    public class FolkloreSapientSpecies
    {
        public int FolkloreId { get; set; }
        public virtual Folklore? Folklore { get; set; }

        public int SapientSpeciesId { get; set; }
        public virtual SapientSpecies? SapientSpecies { get; set; }
    }
}
