using static ChronicleKeeper.Core.Enums.EcosystemEnums;

namespace ChronicleKeeper.Core.Entities.Geography.Ecosystems
{
    public class CaveEcosystem : Ecosystem
    {
        public double Depth { get; set; } // in meters
        public CaveType Type { get; set; } // Enum: Limestone, Lava Tube, etc.
    }
}
