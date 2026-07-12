using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Geography
{
    /// <summary>
    /// TPH subtype of Location (discriminated by the existing LocationType.District value).
    /// Hierarchy rides on the inherited ParentLocationId/SubLocations, not a dedicated FK.
    /// </summary>
    public class District : Location
    {
        public string DistrictType { get; set; } = string.Empty;  // npr. stambeni, poslovni, industrijski

        [NotMapped]
        public City? City => ParentLocation as City;
    }
}
