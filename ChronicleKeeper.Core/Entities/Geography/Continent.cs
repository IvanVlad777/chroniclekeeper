using ChronicleKeeper.Core.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Geography
{
    /// <summary>
    /// TPH subtype of Location (discriminated by the existing LocationType.Continent value).
    /// Hierarchy rides on the inherited ParentLocationId/SubLocations, not a dedicated FK.
    /// </summary>
    public class Continent : Location
    {
        public string? ContinentSpecifics { get; set; }

        [NotMapped]
        public IEnumerable<Region> Regions => SubLocations.OfType<Region>();
    }
}
