using ChronicleKeeper.Core.Entities.Geography;
//using ChronicleKeeper.Core.Entities.Professions;
//using ChronicleKeeper.Core.Entities.Social.Cultures;
//using ChronicleKeeper.Core.Entities.Social.Religions;
using static ChronicleKeeper.Core.Enums.CreatureEnums;

namespace ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient
{
    /// <summary>
    /// A sapient (reasoning) species — now a concrete <see cref="Creature"/> TPH subtype
    /// (discriminator "Sapient"), so it inherits the taxonomy (ParentCreature/Subspecies),
    /// History, physical stats, habitats and Mutations from Creature.
    /// </summary>
    public class SapientSpecies : Creature
    {
        public string CommonName { get; set; } = string.Empty; // e.g., "Elf", "Orc", "Dwarf"
        public string ScientificName { get; set; } = string.Empty; // Optional for realism
        public bool IsHumanoid { get; set; } // True = Human-like body structure
        public string Lifespan { get; set; } = string.Empty; // Freeform, e.g. "~300 years"

        public SapientType SapientType { get; set; }

        public virtual ICollection<Race> Races { get; set; } = new List<Race>();

        public virtual ICollection<RegionSapientSpecies> NativeRegions { get; set; } = new List<RegionSapientSpecies>();

        //public virtual ICollection<Character> Characters { get; set; } = new List<Character>(); // TODO: expose if the inverse navigation becomes useful
        // Reverse read navs for the join tables owned by Profession/Culture/Folklore (typed as the
        // join entity, not the target — the join-entity convention).
        public virtual ICollection<Professions.ProfessionSapientSpecies> FrequentOccupations { get; set; } = new List<Professions.ProfessionSapientSpecies>();
        public virtual ICollection<Social.Cultures.FolkloreSapientSpecies> Folklore { get; set; } = new List<Social.Cultures.FolkloreSapientSpecies>();
        public virtual ICollection<Social.Cultures.CultureSapientSpecies> Cultures { get; set; } = new List<Social.Cultures.CultureSapientSpecies>();
    }
}
