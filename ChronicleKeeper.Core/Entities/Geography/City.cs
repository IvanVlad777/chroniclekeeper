using ChronicleKeeper.Core.Entities.Social.Politics;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Geography
{
    /// <summary>
    /// TPH subtype of Location (discriminated by the existing LocationType.City value).
    /// Hierarchy rides on the inherited ParentLocationId/SubLocations, not a dedicated FK —
    /// a City's parent Country/Region is optional, matching "standalone or part of a country".
    /// </summary>
    public class City : Location
    {
        public bool IsCapital { get; set; }

        public int? GovernmentSystemId { get; set; }
        public GovernmentSystem? GovernmentSystem { get; set; }

        public int? LegalSystemId { get; set; }
        public LegalSystem? LegalSystem { get; set; }

        public int? EducationSystemId { get; set; }
        public Social.Education.EducationSystem? EducationSystem { get; set; }

        //public Army? Army { get; set; } // TODO: Uncomment when Army entity is revived
        //public EconomicSystem? EconomicSystem { get; set; } // TODO: Uncomment when EconomicSystem entity is revived
        //public ICollection<Industry> MajorIndustries { get; set; } = new List<Industry>(); // TODO: Uncomment when Industry entity is revived
        //public ICollection<Corporation> Corporations { get; set; } = new List<Corporation>(); // TODO: Uncomment when Corporation entity is revived
        //public ICollection<Guild> Guilds { get; set; } = new List<Guild>(); // TODO: Uncomment when Guild entity is revived
        //public ICollection<CulturalInstitution> CulturalInstitutions { get; set; } = new List<CulturalInstitution>(); // TODO: Uncomment when CulturalInstitution entity is revived
        //public ICollection<TradeRoute> TradeRoutes { get; set; } = new List<TradeRoute>(); // TODO: Uncomment when TradeRoute entity is revived
        //public ICollection<Creatures.Creature> Creature { get; set; } = new List<Creatures.Creature>(); // TODO: Uncomment when Creature entity is revived

        //public ICollection<PoliticalParty> PoliticalParties { get; set; } = new List<PoliticalParty>(); // TODO: Many-to-many cross-link, needs join entity — deferred, same pattern as Culture<->Nation
        //public ICollection<Social.Cultures.Culture> PredominantCultures { get; set; } = new List<Social.Cultures.Culture>(); // TODO: Many-to-many cross-link, needs join entity — deferred, same pattern as Culture<->Nation
        //public ICollection<Social.Nationality.Nation> Nations { get; set; } = new List<Social.Nationality.Nation>(); // TODO: Many-to-many cross-link, needs join entity — deferred, same pattern as Culture<->Nation
        //public ICollection<Social.Religions.Religion> Religion { get; set; } = new List<Social.Religions.Religion>(); // TODO: Many-to-many cross-link, needs join entity — deferred, same pattern as Culture<->Nation

        [NotMapped]
        public Country? Country => ParentLocation as Country;

        [NotMapped]
        public IEnumerable<District> Districts => SubLocations.OfType<District>();
    }
}
