using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Characters.Abilities;
using ChronicleKeeper.Core.Entities.Characters.CharacterInfo;
using ChronicleKeeper.Core.Entities.Characters.Equipment;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Professions;
using ChronicleKeeper.Core.Entities.Social;
using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Entities.Social.Nationality;
using ChronicleKeeper.Core.Entities.Social.Religions;
using ChronicleKeeper.Core.Entities.Social.Structure;
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
        public virtual History? History { get; set; }
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
        public int SapientSpeciesId { get; set; }
        public virtual SapientSpecies SapientSpecies { get; set; } = null!;

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
        public int? FatherId { get; set; }
        public virtual Character? Father { get; set; }

        public int? MotherId { get; set; }
        public virtual Character? Mother { get; set; }

        public virtual ICollection<Character> Siblings { get; set; } = new List<Character>(); // ✅ Siblings

        //[ForeignKey("Nation")]
        public int? NationId { get; set; }
        public virtual Nation? Nation { get; set; }

        //[ForeignKey("SocialClass")]
        public int? SocialClassId { get; set; }
        public SocialClass? SocialClass { get; set; }

        //[ForeignKey("Religion")]
        public int? ReligionId { get; set; }
        public virtual Religion? Religion { get; set; }


        // Personality & Background
        public BackgroundInfo Background { get; set; } = new BackgroundInfo();
        public PersonalityInfo Personality { get; set; } = new PersonalityInfo();

        // Abilities & Skills
        public virtual ICollection<Ability> Abilities { get; set; } = new List<Ability>();

        // Education & Career
        //[ForeignKey("Profession")]
        public int? ProfessionId { get; set; }
        public virtual Profession? Profession { get; set; }

        public virtual ICollection<EducationRecord> Educations { get; set; } = new List<EducationRecord>();
        public virtual ICollection<Specialisation> Specialisations { get; set; } = new List<Specialisation>();

        // Relationships & Social Interactions
        //public virtual ICollection<CharacterRelationship> Relationships { get; set; } = new List<CharacterRelationship>();

        // Equipment & Inventory
        public virtual ICollection<Item> Equipments { get; set; } = new List<Item>();
        public ICollection<Clothing> Clothing { get; set; } = new List<Clothing>();

        // Hobbies & Interests
        public virtual ICollection<Hobby> Hobbies { get; set; } = new List<Hobby>();
        public virtual ICollection<Faction> Factions { get; set; } = new List<Faction>();
    }

}
