using static ChronicleKeeper.Core.Enums.EcosystemEnums;

namespace ChronicleKeeper.Core.Entities.Geography.Ecosystems
{
    public class ForestEcosystem : Ecosystem
    {
        public ForestType ForestKind { get; set; } // Enum: Rainforest, Boreal, etc.
    }
}
