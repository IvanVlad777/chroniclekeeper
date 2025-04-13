using ChronicleKeeper.Core.Entities.Social;
using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Entities.Social.Economy;
using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Entities.Social.Military;
using ChronicleKeeper.Core.Entities.Social.Nationality;
using ChronicleKeeper.Core.Entities.Social.Politics;
using ChronicleKeeper.Core.Entities.Social.Religions;


namespace ChronicleKeeper.Core.Entities.Geography
{
    public class Country : Location
    {
        //[ForeignKey("Region")]
        //public int? RegionId { get; set; }
        //public Region? Region { get; set; }

        //[ForeignKey("GovernmentSystem")]
        public int? GovernmentSystemId { get; set; }
        public GovernmentSystem? GovernmentSystem { get; set; }
        public ICollection<PoliticalParty> PoliticalParties { get; set; } = new List<PoliticalParty>();

        public ICollection<City> Cities { get; set; } = new List<City>();


        public int? PrimaryNationId { get; set; } // Zasad samo ID
        public ICollection<Nation> Nations { get; set; } = new List<Nation>();


        //[ForeignKey("LegalSystem")]
        public int? LegalSystemId { get; set; }
        public LegalSystem? LegalSystem { get; set; }

        //[ForeignKey("EconomicSystem")]
        public int? EconomicSystemId { get; set; }
        public EconomicSystem? EconomicSystem { get; set; }

        //[ForeignKey("EducationSystem")]
        public int? EducationSystemId { get; set; }
        public EducationSystem? EducationSystem { get; set; }


        public ICollection<MilitaryOrganization> MilitaryOrganizations { get; set; } = new List<MilitaryOrganization>();
        public ICollection<Industry> MajorIndustries { get; set; } = new List<Industry>();
        public ICollection<Corporation> Corporations { get; set; } = new List<Corporation>();
        public ICollection<Guild> Guilds { get; set; } = new List<Guild>();
        public ICollection<Faction> Factions { get; set; } = new List<Faction>();


        public ICollection<Culture> PredominantCultures { get; set; } = new List<Culture>();
        public ICollection<Religion> Religions { get; set; } = new List<Religion>();

        //public ICollection<TradeRoute> TradeRoutes { get; set; } = new List<TradeRoute>();
    }

}
