using ChronicleKeeper.Core.Entities.Geography.Creatures;
using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Entities.Social.Economy;
using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Entities.Social.Military;
using ChronicleKeeper.Core.Entities.Social.Nationality;
using ChronicleKeeper.Core.Entities.Social.Politics;
using ChronicleKeeper.Core.Entities.Social.Religions;


namespace ChronicleKeeper.Core.Entities.Geography
{
    public class City : Location
    {
        //[ForeignKey("Country")]
        //public int CountryId { get; set; }
        //public Country Country { get; set; } = null!;

        public bool IsCapital { get; set; }  // Označava je li glavni grad

        //[ForeignKey("GovernmentSystem")]
        public int? GovernmentSystemId { get; set; }
        public GovernmentSystem? GovernmentSystem { get; set; }

        //[ForeignKey("LegalSystem")]
        public int LegalSystemId { get; set; }
        public LegalSystem LegalSystem { get; set; } = null!;

        public ICollection<PoliticalParty> PoliticalParties { get; set; } = new List<PoliticalParty>();

        //[ForeignKey("Army")]
        //public int? ArmyId { get; set; }
        public Army? Army { get; set; }


        //[ForeignKey("EconomicSystem")]
        public int? EconomicSystemId { get; set; }
        public EconomicSystem? EconomicSystem { get; set; }


        //[ForeignKey("TaxationSystem")]
        //public int TaxationSystemId { get; set; }
        //public TaxationSystem? TaxationSystem { get; set; }

        //[ForeignKey("BankingSystem")]
        //public int? BankingSystemId { get; set; }
        //public BankingSystem? BankingSystem { get; set; }


        //[ForeignKey("EducationSystem")]
        public int? EducationSystemId { get; set; }
        public EducationSystem? EducationSystem { get; set; }

        public ICollection<Industry> MajorIndustries { get; set; } = new List<Industry>();
        public ICollection<Corporation> Corporations { get; set; } = new List<Corporation>(); // ✅ Businesses in this country
        public ICollection<Guild> Guilds { get; set; } = new List<Guild>(); // ✅ Trade/labor organizations
        public ICollection<District> Districts { get; set; } = new List<District>();
        public ICollection<CulturalInstitution> CulturalInstitutions { get; set; } = new List<CulturalInstitution>(); // ✅ Museums, Theaters, Libraries
        //public ICollection<TradeRoute> TradeRoutes { get; set; } = new List<TradeRoute>(); // ✅ Trade connections
        public ICollection<Culture> PredominantCultures { get; set; } = new List<Culture>(); // ✅ Cultural influence in the city
        public ICollection<Creature> Creature { get; set; } = new List<Creature>();
        public ICollection<Nation> Nations { get; set; } = new List<Nation>();
        public ICollection<Religion> Religion { get; set; } = new List<Religion>();
    }
}
