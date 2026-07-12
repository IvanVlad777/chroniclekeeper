using ChronicleKeeper.Core.Entities.Social.Politics;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Geography
{
    /// <summary>
    /// TPH subtype of Location (discriminated by the existing LocationType.Country value).
    /// Hierarchy rides on the inherited ParentLocationId/SubLocations, not a dedicated FK.
    /// </summary>
    public class Country : Location
    {
        public int? GovernmentSystemId { get; set; }
        public GovernmentSystem? GovernmentSystem { get; set; }

        public int? LegalSystemId { get; set; }
        public LegalSystem? LegalSystem { get; set; }

        public int? PrimaryNationId { get; set; } // Zasad samo ID

        public int? EducationSystemId { get; set; }
        public Social.Education.EducationSystem? EducationSystem { get; set; }

        //public EconomicSystem? EconomicSystem { get; set; } // TODO: Uncomment when EconomicSystem entity is revived
        //public ICollection<MilitaryOrganization> MilitaryOrganizations { get; set; } = new List<MilitaryOrganization>(); // TODO: Uncomment when MilitaryOrganization entity is revived
        //public ICollection<Industry> MajorIndustries { get; set; } = new List<Industry>(); // TODO: Uncomment when Industry entity is revived
        //public ICollection<Corporation> Corporations { get; set; } = new List<Corporation>(); // TODO: Uncomment when Corporation entity is revived
        //public ICollection<Guild> Guilds { get; set; } = new List<Guild>(); // TODO: Uncomment when Guild entity is revived
        //public ICollection<TradeRoute> TradeRoutes { get; set; } = new List<TradeRoute>(); // TODO: Uncomment when TradeRoute entity is revived

        //public ICollection<PoliticalParty> PoliticalParties { get; set; } = new List<PoliticalParty>(); // TODO: Many-to-many cross-link, needs join entity — deferred, same pattern as Culture<->Nation
        //public ICollection<Nation> Nations { get; set; } = new List<Nation>(); // TODO: Many-to-many cross-link, needs join entity — deferred, same pattern as Culture<->Nation
        //public ICollection<Faction> Factions { get; set; } = new List<Faction>(); // TODO: Many-to-many cross-link, needs join entity — deferred, same pattern as Culture<->Nation
        //public ICollection<Social.Cultures.Culture> PredominantCultures { get; set; } = new List<Social.Cultures.Culture>(); // TODO: Many-to-many cross-link, needs join entity — deferred, same pattern as Culture<->Nation
        //public ICollection<Social.Religions.Religion> Religions { get; set; } = new List<Social.Religions.Religion>(); // TODO: Many-to-many cross-link, needs join entity — deferred, same pattern as Culture<->Nation

        [NotMapped]
        public Region? Region => ParentLocation as Region;

        [NotMapped]
        public IEnumerable<City> Cities => SubLocations.OfType<City>();
    }
}
