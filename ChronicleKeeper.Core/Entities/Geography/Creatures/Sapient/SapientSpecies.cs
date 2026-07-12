using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography;
//using ChronicleKeeper.Core.Entities.Professions;
//using ChronicleKeeper.Core.Entities.Social.Cultures;
//using ChronicleKeeper.Core.Entities.Social.Religions;
//using static ChronicleKeeper.Core.Enums.CreatureEnums;

namespace ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient
{
    // TODO: Re-parent to Creature when the Creature hierarchy is revived (Creature drags
    // Ecosystem/City/Mutation navigations into the EF model, so it stays dormant for now).
    public class SapientSpecies : LoreEntity
    {
        public string CommonName { get; set; } = string.Empty; // e.g., "Elf", "Orc", "Dwarf"
        public string ScientificName { get; set; } = string.Empty; // Optional for realism
        public bool IsHumanoid { get; set; } // True = Human-like body structure
        public string Lifespan { get; set; } = string.Empty; // Freeform, e.g. "~300 years"

        public virtual ICollection<Race> Races { get; set; } = new List<Race>();

        public virtual ICollection<RegionSapientSpecies> NativeRegions { get; set; } = new List<RegionSapientSpecies>();

        //public SapientType SapientType { get; set; } // TODO: Uncomment when Creature hierarchy is revived
        //public virtual ICollection<Character> Characters { get; set; } = new List<Character>(); // TODO: expose if the inverse navigation becomes useful
        //public ICollection<Profession> FrequentOccupations { get; set; } = new List<Profession>(); // TODO: Uncomment when Profession entity is revived
        //public ICollection<Folklore> Folklore { get; set; } = new List<Folklore>(); // TODO: Uncomment when Folklore entity is revived
        //public ICollection<Culture> Cultures { get; set; } = new List<Culture>(); // TODO: Uncomment when Culture entity is revived
    }
}
