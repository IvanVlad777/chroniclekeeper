using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.Social.Nationality;
using static ChronicleKeeper.Core.Enums.SocietyEnums;
using System.ComponentModel.DataAnnotations.Schema;
using ChronicleKeeper.Core.Entities.Social.Religions;
using ChronicleKeeper.Core.Interfaces;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.Entities.Social.Structure;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class Culture : ILoreEntity
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

        //public virtual ICollection<Language> Languages { get; set; } = new List<Language>(); // ✅ Languages spoken
        //[ForeignKey("Language")]
        public int LanguageId { get; set; }
        public Language? Language { get; set; }

        public virtual ICollection<Custom> Customs { get; set; } = new List<Custom>(); // ✅ Social customs
        public virtual ICollection<Clothing> Clothing { get; set; } = new List<Clothing>();
        public virtual ICollection<ArtForm> ArtForms { get; set; } = new List<ArtForm>(); // ✅ Music, dance, visual arts
        public virtual ICollection<Cuisine> Cuisines { get; set; } = new List<Cuisine>(); // ✅ Food preferences
        public virtual ICollection<Tradition> Traditions { get; set; } = new List<Tradition>(); // ✅ Religious/social traditions
        public virtual ICollection<ArchitectureStyle> ArchitectureStyles { get; set; } = new List<ArchitectureStyle>(); // ✅ Unique architectural features
        public virtual ICollection<Folklore> Folktales { get; set; } = new List<Folklore>(); // ✅ Legends and stories
        public virtual ICollection<Myth> Myths { get; set; } = new List<Myth>(); // ✅ Myths of the culture
        public virtual ICollection<CulturalInstitution> CulturalInstitutions { get; set; } = new List<CulturalInstitution>(); // ✅ Museums, Theaters, Libraries

        //[ForeignKey("Religion")]
        public int? ReligionId { get; set; }
        public Religion? Religion { get; set; }

        public string CommonValues { get; set; } = string.Empty;
        public bool HasOralTradition { get; set; } // Whether history is passed down verbally
        public string SocialStructure { get; set; } = string.Empty;
        public XenophobiaLevel XenophobiaLevel { get; set; } // 1 = Vrlo otvoreni, 5 = Ekstremno ksenofobni - enum
        public TechnologicalLevel TechnologicalLevel { get; set; } // enum
        public string ConflictResolution { get; set; } = string.Empty; // Rat, diplomacija, dueli itd.
        public virtual ICollection<Nation> Nation { get; set; } = new List<Nation>(); // Cultures tied to races
        public virtual ICollection<Country> PredominantInCountries { get; set; } = new List<Country>(); // ✅ Cultures exist in countries
        public virtual ICollection<City> PredominantInCities { get; set; } = new List<City>(); // ✅ Cultures exist in countries
        public virtual ICollection<SapientSpecies> PracticedBySpecies { get; set; } = new List<SapientSpecies>(); // ✅ Species following this culture
        public ICollection<SocialClass> InfluencedSocialClasses { get; set; } = new List<SocialClass>();

    }
}
