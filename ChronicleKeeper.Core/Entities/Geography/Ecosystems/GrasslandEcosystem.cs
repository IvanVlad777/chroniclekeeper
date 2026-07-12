using static ChronicleKeeper.Core.Enums.EcosystemEnums;

namespace ChronicleKeeper.Core.Entities.Geography.Ecosystems
{
    public class GrasslandEcosystem : Ecosystem
    {
        public GrasslandType GrasslandKind { get; set; } // Enum: Prairie, Steppe, Savannah, etc.
    }
}
