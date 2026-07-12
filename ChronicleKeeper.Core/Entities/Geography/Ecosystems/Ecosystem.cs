using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Geography.Ecosystems
{
    /// <summary>
    /// TPH subtype of Location (shadow discriminator "LocationSubtype", see LocationConfiguration).
    /// Hierarchy (incl. Region containment) rides on the inherited ParentLocationId/SubLocations —
    /// no dedicated RegionId FK, mirrors the Country/City/District convenience-accessor pattern.
    /// Creature linkage lives on Creature.Habitants (CreatureEcosystem join), not here — mirrors
    /// CreatureCity's asymmetric ownership (Creature owns the collection, target side doesn't).
    /// </summary>
    public abstract class Ecosystem : Location
    {
        public string UniqueFeatures { get; set; } = string.Empty;

        [NotMapped]
        public Region? Region => ParentLocation as Region;
    }
}
