using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Social.Religions;
using static ChronicleKeeper.Core.Enums.SocietyEnums;
//using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
//using ChronicleKeeper.Core.Entities.Geography;
//using ChronicleKeeper.Core.Entities.Social.Nationality;
//using ChronicleKeeper.Core.Entities.Social.Structure;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class Culture : LoreEntity
    {
        public int LanguageId { get; set; }
        public Language? Language { get; set; }

        public int? ReligionId { get; set; }
        public Religion? Religion { get; set; }

        public string CommonValues { get; set; } = string.Empty;
        public bool HasOralTradition { get; set; } // Whether history is passed down verbally
        public string SocialStructure { get; set; } = string.Empty;
        public XenophobiaLevel XenophobiaLevel { get; set; } // 1 = Vrlo otvoreni, 5 = Ekstremno ksenofobni - enum
        public TechnologicalLevel TechnologicalLevel { get; set; } // enum
        public string ConflictResolution { get; set; } = string.Empty; // Rat, diplomacija, dueli itd.

        //public virtual ICollection<Custom> Customs { get; set; } = new List<Custom>(); // TODO: Uncomment when Custom entity is revived
        //public virtual ICollection<Clothing> Clothing { get; set; } = new List<Clothing>(); // TODO: Uncomment when Clothing entity is revived
        //public virtual ICollection<ArtForm> ArtForms { get; set; } = new List<ArtForm>(); // TODO: Uncomment when ArtForm entity is revived
        //public virtual ICollection<Cuisine> Cuisines { get; set; } = new List<Cuisine>(); // TODO: Uncomment when Cuisine entity is revived
        //public virtual ICollection<Tradition> Traditions { get; set; } = new List<Tradition>(); // TODO: Uncomment when Tradition entity is revived
        //public virtual ICollection<ArchitectureStyle> ArchitectureStyles { get; set; } = new List<ArchitectureStyle>(); // TODO: Uncomment when ArchitectureStyle entity is revived
        //public virtual ICollection<Folklore> Folktales { get; set; } = new List<Folklore>(); // TODO: Uncomment when Folklore entity is revived
        //public virtual ICollection<Myth> Myths { get; set; } = new List<Myth>(); // TODO: Uncomment when Myth entity is revived
        //public virtual ICollection<CulturalInstitution> CulturalInstitutions { get; set; } = new List<CulturalInstitution>(); // TODO: Uncomment when CulturalInstitution entity is revived

        // TODO: Many-to-many cross-links to already-mapped entities (Nation, SapientSpecies, SocialClass) —
        // deferred to a dedicated pass so the join-table shape is decided once, holistically.
        //public virtual ICollection<Nation> Nation { get; set; } = new List<Nation>();
        //public virtual ICollection<SapientSpecies> PracticedBySpecies { get; set; } = new List<SapientSpecies>();
        //public ICollection<SocialClass> InfluencedSocialClasses { get; set; } = new List<SocialClass>();

        //public virtual ICollection<Country> PredominantInCountries { get; set; } = new List<Country>(); // TODO: Uncomment when Country entity is revived
        //public virtual ICollection<City> PredominantInCities { get; set; } = new List<City>(); // TODO: Uncomment when City entity is revived
    }
}
