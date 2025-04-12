using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.Professions;
using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Entities.Social.Religions;
using static ChronicleKeeper.Core.Enums.CreatureEnums;

namespace ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient
{
    public class SapientSpecies : Creature
    {
        public string CommonName { get; set; } = string.Empty; // e.g., "Elf", "Orc", "Dwarf"
        public string ScientificName { get; set; } = string.Empty; // Optional for realism
        public bool IsHumanoid { get; set; } // True = Human-like body structure
        public SapientType SapientType { get; set; } // Enum: Humanoid, Beast-like, Elemental, etc.
        public ICollection<Race> Races { get; set; } = new List<Race>();
        public ICollection<Region> NativeRegions { get; set; } = new List<Region>();
        public virtual ICollection<Character> Characters { get; set; } = new List<Character>();
        public ICollection<Profession> FrequentOccupations { get; set; } = new List<Profession>();
        public ICollection<Folklore> Folklore { get; set; } = new List<Folklore>();
        public ICollection<Culture> Cultures { get; set; } = new List<Culture>();
    }
}
