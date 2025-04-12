using static ChronicleKeeper.Core.Enums.EcosystemEnums;

namespace ChronicleKeeper.Core.Entities.Geography.Ecosystems
{
    public class GrasslandEcosystem : Ecosystem
    {
        public GrasslandType Type { get; set; } // Enum: Prairie, Steppe, Savannah, etc.
    }
}
