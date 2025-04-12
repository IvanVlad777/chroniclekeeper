using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;

namespace ChronicleKeeper.Core.Entities.Geography
{
    public class Region : Location
    {
        //[ForeignKey("Continent")]
        //public int ContinentId { get; set; }
        //public Continent Continent { get; set; } = null!;

        //public ICollection<Country> Countries { get; set; } = new List<Country>();

        // ✅ Now Regions can have multiple biomes
        //public ICollection<Ecosystem> Ecosystems { get; set; } = new List<Ecosystem>();
        public ICollection<SapientSpecies> OriginOfSapientSpecies { get; set; } = new List<SapientSpecies>();
        public string? RegionSpecifics { get; set; }
    }
}
