using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Geography
{
    /// <summary>
    /// TPH subtype of Location (discriminated by the existing LocationType.Region value).
    /// Hierarchy rides on the inherited ParentLocationId/SubLocations, not a dedicated FK.
    /// </summary>
    public class Region : Location
    {
        //public ICollection<Ecosystem> Ecosystems { get; set; } = new List<Ecosystem>(); // TODO: Uncomment when Ecosystem entity is revived

        public virtual ICollection<RegionSapientSpecies> OriginOfSapientSpecies { get; set; } = new List<RegionSapientSpecies>();
        public string? RegionSpecifics { get; set; }

        [NotMapped]
        public Continent? Continent => ParentLocation as Continent;

        [NotMapped]
        public IEnumerable<Country> Countries => SubLocations.OfType<Country>();
    }
}
