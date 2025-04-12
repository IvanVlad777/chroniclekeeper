using static ChronicleKeeper.Core.Enums.ReliefShapeEnums;

namespace ChronicleKeeper.Core.Entities.Geography.Ecosystems
{
    public abstract class WaterEcosystem : Ecosystem
    {
        public double Depth { get; set; } // Average depth (m)
        public WaterBodyType Type { get; set; } // Enum: River, Lake, Ocean, etc.
    }
}
