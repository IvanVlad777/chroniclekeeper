using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Geography.Ecosystems
{
    /// <summary>
    /// A Mountain's MountainRange is modeled via the inherited ParentLocationId (validated to be a
    /// MountainRange, see LocationValidation.AllowedParentTypes), not a dedicated FK.
    /// </summary>
    public class MountainEcosystem : Ecosystem
    {
        public double MaxElevation { get; set; } // in meters
        public double Prominence { get; set; } // in meters

        [NotMapped]
        public MountainRange? MountainRange => ParentLocation as MountainRange;
    }
}
