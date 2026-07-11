using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Characters.CharacterInfo;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Entities.Social;
using ChronicleKeeper.Core.Entities.Tags;
using ChronicleKeeper.Core.Entities.Characters.Abilities;
using ChronicleKeeper.Core.Entities.Characters.Equipment;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Professions;
//using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Entities.Social.Nationality;
using ChronicleKeeper.Core.Entities.Social.Religions;
using ChronicleKeeper.Core.Entities.Social.Structure;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Characters
{
    public class Character : LoreEntity
    {
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
        public int? SapientSpeciesId { get; set; }
        public virtual SapientSpecies? SapientSpecies { get; set; }

        public int? RaceId { get; set; }
        public virtual Race? Race { get; set; } // Race must belong to the character's species — validated in command handlers

        public bool IsArtificial { get; set; } = false; // True if a robot, AI, cyborg, etc.

        // Family & Origin
        public int? FatherId { get; set; }
        public virtual Character? Father { get; set; }

        public int? MotherId { get; set; }
        public virtual Character? Mother { get; set; }

        public int? NationId { get; set; }
        public virtual Nation? Nation { get; set; }

        public int? SocialClassId { get; set; }
        public virtual SocialClass? SocialClass { get; set; }

        public int? ReligionId { get; set; }
        public virtual Religion? Religion { get; set; }

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        // Personality & Background
        //public BackgroundInfo Background { get; set; } = new BackgroundInfo(); // TODO: Uncomment when BackgroundInfo entity is revived
        //public PersonalityInfo Personality { get; set; } = new PersonalityInfo(); // TODO: Uncomment when PersonalityInfo entity is revived

        // Abilities & Skills
        public virtual ICollection<CharacterAbility> Abilities { get; set; } = new List<CharacterAbility>();

        // Education & Career
        public int? ProfessionId { get; set; }
        public virtual Profession? Profession { get; set; }

        public virtual ICollection<EducationRecord> Educations { get; set; } = new List<EducationRecord>();
        //public virtual ICollection<Specialisation> Specialisations { get; set; } = new List<Specialisation>(); // TODO: Uncomment when Character many-to-many cross-links are revived

        // Relationships & Social Interactions
        public virtual ICollection<CharacterRelationship> Relationships { get; set; } = new List<CharacterRelationship>();
        public virtual ICollection<FactionMember> Memberships { get; set; } = new List<FactionMember>();

        // Tags
        public virtual ICollection<CharacterTag> Tags { get; set; } = new List<CharacterTag>();

        // Equipment & Inventory
        public virtual ICollection<Item> Equipments { get; set; } = new List<Item>();
        //public ICollection<Clothing> Clothing { get; set; } = new List<Clothing>(); // TODO: Uncomment when Clothing entity is revived

        // Hobbies & Interests
        //public virtual ICollection<Hobby> Hobbies { get; set; } = new List<Hobby>(); // TODO: Uncomment when Hobby entity is revived
    }
}
