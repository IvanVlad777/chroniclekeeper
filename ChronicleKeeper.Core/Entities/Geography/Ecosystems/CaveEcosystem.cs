using static ChronicleKeeper.Core.Enums.EcosystemEnums;

namespace ChronicleKeeper.Core.Entities.Geography.Ecosystems
{
    public class CaveEcosystem : Ecosystem
    {
        public double CaveDepth { get; set; } // in meters
        public CaveType CaveKind { get; set; } // Enum: Limestone, Lava Tube, etc.
    }
}
