using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Animals;
using ChronicleKeeper.Core.Entities.Geography.Ecosystems;
using ChronicleKeeper.Core.Enums;
using static ChronicleKeeper.Core.Enums.GlobalEnums;

namespace ChronicleKeeper.Core.Entities.Geography.Creatures.Plants
{
    public class Plant : Creature
    {
        public PlantType PlantType { get; set; } // Tree, Shrub, Herb, etc.
        public string ScientificName { get; set; } = string.Empty; // Botanical name
        public bool IsMedicinal { get; set; } // True if it has healing properties
        public bool IsPoisonous { get; set; } // True if toxic to humans/animals
        public SunlightRequirement Sunlight { get; set; }
        public SoilType PreferredSoil { get; set; }
        public TemperatureRange TemperatureRange { get; set; }
        public Rarity Rarity { get; set; }

        public bool IsBioluminescent { get; set; } // Svijetli li u mraku?
        public bool IsCarnivorous { get; set; } // Hvata li plijen?
        public bool HasRegenerativeProperties { get; set; } // Može li se samoregenerirati?
        public bool CanMove { get; set; } // Može li biljka hodati?

        public string SpecialProperties { get; set; } = string.Empty; // "Heals wounds instantly", "Curses those who touch it"
        public string MythologicalSignificance { get; set; } = string.Empty;

        public bool IsSymbiotic { get; set; } // Živi li u simbiozi s drugim vrstama?
        public bool IsParasitic { get; set; } // Isisava li energiju iz domaćina?

    }
}
