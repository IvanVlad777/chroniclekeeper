using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography.Ecosystems;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Geography.Creatures.Fungi
{
    public class Fungus : Creature
    {
        //public FungusType Type { get; set; } // Tree, Shrub, Herb, etc.
        public string ScientificName { get; set; } = string.Empty; // Botanical name
        public bool IsMedicinal { get; set; } // True if it has healing properties
        public bool IsPoisonous { get; set; } // True if toxic to humans/animals
        public bool IsEdible { get; set; } // True if it can be eaten
        public bool IsHallucinogenic { get; set; } // True if it has psychoactive effects

        public bool IsBioluminescent { get; set; } // True ako svijetli u mraku
        public bool HasMutagenicProperties { get; set; } // Može li izazvati mutacije?
        public bool IsSymbiotic { get; set; } // Živi li u simbiozi s drugim vrstama?
        public bool CanCommunicate { get; set; }

        public string SpecialProperties { get; set; } = string.Empty; // Npr. "Teleportation spores", "Mind control toxins"
        public string MythologicalSignificance { get; set; } = string.Empty;

    }
}
