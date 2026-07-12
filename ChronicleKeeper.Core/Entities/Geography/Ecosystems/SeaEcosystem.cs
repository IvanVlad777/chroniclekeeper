using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Geography.Ecosystems
{
    /// <summary>
    /// A Sea's Ocean is modeled via the inherited ParentLocationId (validated to be an
    /// OceanEcosystem, see LocationValidation.AllowedParentTypes), not a dedicated FK —
    /// same convenience-accessor pattern as Country/City/District.
    /// </summary>
    public class SeaEcosystem : WaterEcosystem
    {
        [NotMapped]
        public OceanEcosystem? Ocean => ParentLocation as OceanEcosystem;
    }
}
