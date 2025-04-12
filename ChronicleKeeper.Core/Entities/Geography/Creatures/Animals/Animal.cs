using ChronicleKeeper.Core.Entities.Geography.Creatures.Fungi;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Plants;
using ChronicleKeeper.Core.Entities.Geography.Ecosystems;
using System.ComponentModel.DataAnnotations.Schema;
using static ChronicleKeeper.Core.Enums.CreatureEnums;

namespace ChronicleKeeper.Core.Entities.Geography.Creatures.Animals
{
    public class Animal : Creature
    {
        public DietType Diet { get; set; } // Herbivore, Carnivore, Omnivore
        public bool IsDomesticated { get; set; } // True = Livestock or pets
        public int NumberOfLegs { get; set; } // 2 (ptice), 4 (većina sisavaca), 6+ (fantasy stvorenja)
        public bool HasWings { get; set; } // True ako može letjeti
        public bool HasMultipleHeads { get; set; } // Za zmije s dvije glave, zmajeve itd.
        public bool HasRegeneration { get; set; } // Npr. gušteri, magična bića

        // ✅ Status u svijetu (mitološko, sveto, prokletstvo...)
        public bool IsSacred { get; set; } // Je li poštovano u religijama
        public bool IsMythical { get; set; } // Postoji li samo u legendama?
        public bool IsEndangered { get; set; } // Skoro izumrla vrsta?
        public string Intelligence { get; set; } = string.Empty;

        // ✅ Magične/Sci-Fi sposobnosti
        public string SpecialAbilities { get; set; } = string.Empty; // "Telepathy, Fire Breath"

        // ✅ Društveni obrasci
        public bool IsPackAnimal { get; set; } // Živi li u čoporima
        public bool IsAggressive { get; set; } // Napada li sve što vidi?
        public bool IsSymbiotic { get; set; } // Živi li u harmoniji s drugim vrstama?

    }
}
