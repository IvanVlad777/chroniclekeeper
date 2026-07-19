using ChronicleKeeper.Core.Entities.Geography.Creatures;
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
        public int? EconomicSystemId { get; set; }
        public Social.Economy.EconomicSystem? EconomicSystem { get; set; }

        // City-owned many-to-many cross-links (join entities in CountryCityCrossLinks.cs).
        // Owner side holds the nav; target entities get no reverse nav (asymmetric convention).
        public virtual ICollection<CityIndustry> Industries { get; set; } = new List<CityIndustry>();
        public virtual ICollection<CityCorporation> Corporations { get; set; } = new List<CityCorporation>();
        public virtual ICollection<CityGuild> Guilds { get; set; } = new List<CityGuild>();
        public virtual ICollection<CityCulturalInstitution> CulturalInstitutions { get; set; } = new List<CityCulturalInstitution>();
        public virtual ICollection<CityPoliticalParty> PoliticalParties { get; set; } = new List<CityPoliticalParty>();
        public virtual ICollection<CityCulture> PredominantCultures { get; set; } = new List<CityCulture>();
        public virtual ICollection<CityNation> Nations { get; set; } = new List<CityNation>();
        public virtual ICollection<CityReligion> Religions { get; set; } = new List<CityReligion>();
        public virtual ICollection<CityCurrency> Currencies { get; set; } = new List<CityCurrency>();
        public virtual ICollection<CityTaxationSystem> TaxationSystems { get; set; } = new List<CityTaxationSystem>();

        // Reverse read-only nav for the Army.CityId FK (armies stationed in this city).
        public virtual ICollection<Social.Military.Army> Armies { get; set; } = new List<Social.Military.Army>();

        // Reverse read-only nav for the creature-owned CreatureCity join (surfaced as a read list
        // on the City detail — no LinkEditor, the Creature owns the write side).
        public virtual ICollection<CreatureCity> InhabitingCreatures { get; set; } = new List<CreatureCity>();
        // NOTE: relation to TradeRoute already exists via the TradeRouteLocation join (TradeRoute owns
        // the nav); City surfaces it read-only through the inherited Location.TradeRouteLinks nav.

        [NotMapped]
        public Country? Country => ParentLocation as Country;

        [NotMapped]
        public IEnumerable<District> Districts => SubLocations.OfType<District>();
    }
}
