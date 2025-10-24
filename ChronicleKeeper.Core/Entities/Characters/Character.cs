using ChronicleKeeper.Core.Entities.Base;
//using ChronicleKeeper.Core.Entities.Characters.Abilities; 
//using ChronicleKeeper.Core.Entities.Characters.CharacterInfo; 
//using ChronicleKeeper.Core.Entities.Characters.Equipment; 
//using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient; 
//using ChronicleKeeper.Core.Entities.HistoryTimelines; 
//using ChronicleKeeper.Core.Entities.Professions; 
//using ChronicleKeeper.Core.Entities.Social; 
//using ChronicleKeeper.Core.Entities.Social.Cultures; 
//using ChronicleKeeper.Core.Entities.Social.Education; 
//using ChronicleKeeper.Core.Entities.Social.Nationality; 
//using ChronicleKeeper.Core.Entities.Social.Religions; 
//using ChronicleKeeper.Core.Entities.Social.Structure; 
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Characters
{
    public class Character : ILoreEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        //public virtual History? History { get; set; } // TODO: Uncomment when History entity is needed
        // Identity
        [Required]
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;

        // Date of Life
        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }

        // Physical Appearance
        public double? Height { get; set; } // cm
        public double? Weight { get; set; } // kg
        public string HairColor { get; set; } = string.Empty;
        public string EyeColor { get; set; } = string.Empty;
        public string SpecialPhysicalFeatures { get; set; } = string.Empty;

        // Biological Traits
        //[Required]
        //[ForeignKey("SapientSpecies")]
        //public int? SapientSpeciesId { get; set; } // TODO: Uncomment when SapientSpecies entity is needed
        //public virtual SapientSpecies? SapientSpecies { get; set; } // TODO: Uncomment when SapientSpecies entity is needed

        //public int? RaceId { get; private set; }
        //public virtual Race? Race { get; private set; } // ✅ Tied to a race within their species
        //public void SetRace(Race race)
        //{
        //    if (race.SapientSpeciesId != this.SapientSpeciesId)
        //    {
        //        throw new InvalidOperationException($"Character cannot have the race '{race.Name}' because it does not belong to the species '{SapientSpecies.Name}'.");
        //    }
        //    this.Race = race;
        //    this.RaceId = race.Id;
        //}

        public bool IsArtificial { get; set; } = false; // ✅ True if a robot, AI, cyborg, etc.

        // Family & Origin
        //public int? FatherId { get; set; } // TODO: Uncomment when family relationships are needed
        //public virtual Character? Father { get; set; } // TODO: Uncomment when family relationships are needed

        //public int? MotherId { get; set; } // TODO: Uncomment when family relationships are needed
        //public virtual Character? Mother { get; set; } // TODO: Uncomment when family relationships are needed

        //public virtual ICollection<Character> Siblings { get; set; } = new List<Character>(); // TODO: Uncomment when family relationships are needed

        //[ForeignKey("Nation")]
        //public int? NationId { get; set; } // TODO: Uncomment when Nation entity is needed
        //public virtual Nation? Nation { get; set; } // TODO: Uncomment when Nation entity is needed

        //[ForeignKey("SocialClass")]
        //public int? SocialClassId { get; set; } // TODO: Uncomment when SocialClass entity is needed
        //public SocialClass? SocialClass { get; set; } // TODO: Uncomment when SocialClass entity is needed

        //[ForeignKey("Religion")]
        //public int? ReligionId { get; set; } // TODO: Uncomment when Religion entity is needed
        //public virtual Religion? Religion { get; set; } // TODO: Uncomment when Religion entity is needed


        // Personality & Background
        //public BackgroundInfo Background { get; set; } = new BackgroundInfo(); // TODO: Uncomment when BackgroundInfo entity is needed
        //public PersonalityInfo Personality { get; set; } = new PersonalityInfo(); // TODO: Uncomment when PersonalityInfo entity is needed

        // Abilities & Skills
        //public virtual ICollection<Ability> Abilities { get; set; } = new List<Ability>(); // TODO: Uncomment when Ability entity is needed

        // Education & Career
        //[ForeignKey("Profession")]
        //public int? ProfessionId { get; set; } // TODO: Uncomment when Profession entity is needed
        //public virtual Profession? Profession { get; set; } // TODO: Uncomment when Profession entity is needed

        //public virtual ICollection<EducationRecord> Educations { get; set; } = new List<EducationRecord>(); // TODO: Uncomment when EducationRecord entity is needed
        //public virtual ICollection<Specialisation> Specialisations { get; set; } = new List<Specialisation>(); // TODO: Uncomment when Specialisation entity is needed

        // Relationships & Social Interactions
        //public virtual ICollection<CharacterRelationship> Relationships { get; set; } = new List<CharacterRelationship>();

        // Equipment & Inventory
        //public virtual ICollection<Item> Equipments { get; set; } = new List<Item>(); // TODO: Uncomment when Item entity is needed
        //public ICollection<Clothing> Clothing { get; set; } = new List<Clothing>(); // TODO: Uncomment when Clothing entity is needed

        // Hobbies & Interests
        //public virtual ICollection<Hobby> Hobbies { get; set; } = new List<Hobby>(); // TODO: Uncomment when Hobby entity is needed
        //public virtual ICollection<Faction> Factions { get; set; } = new List<Faction>(); // TODO: Uncomment when Faction entity is needed
    }

}
