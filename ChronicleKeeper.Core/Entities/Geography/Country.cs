using ChronicleKeeper.Core.Entities.Social.Military;
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

        public int? EconomicSystemId { get; set; }
        public Social.Economy.EconomicSystem? EconomicSystem { get; set; }

        // Country-owned many-to-many cross-links (join entities in CountryCityCrossLinks.cs).
        // Owner side holds the nav; target entities get no reverse nav (asymmetric convention).
        public virtual ICollection<CountryIndustry> Industries { get; set; } = new List<CountryIndustry>();
        public virtual ICollection<CountryCorporation> Corporations { get; set; } = new List<CountryCorporation>();
        public virtual ICollection<CountryGuild> Guilds { get; set; } = new List<CountryGuild>();
        public virtual ICollection<CountryPoliticalParty> PoliticalParties { get; set; } = new List<CountryPoliticalParty>();
        public virtual ICollection<CountryNation> Nations { get; set; } = new List<CountryNation>();
        public virtual ICollection<CountryFaction> Factions { get; set; } = new List<CountryFaction>();
        public virtual ICollection<CountryCulture> PredominantCultures { get; set; } = new List<CountryCulture>();
        public virtual ICollection<CountryReligion> Religions { get; set; } = new List<CountryReligion>();
        public virtual ICollection<CountryCurrency> Currencies { get; set; } = new List<CountryCurrency>();
        public virtual ICollection<CountryTaxationSystem> TaxationSystems { get; set; } = new List<CountryTaxationSystem>();

        // Reverse read-only nav for the org-owned MilitaryOrganizationCountry join (surfaced as a
        // read list on the Country detail — no LinkEditor, the org owns the write side).
        public virtual ICollection<MilitaryOrganizationCountry> MilitaryOrganizations { get; set; } = new List<MilitaryOrganizationCountry>();
        // NOTE: relation to TradeRoute already exists via the TradeRouteLocation join (TradeRoute owns
        // the nav); Country surfaces it read-only through the inherited Location.TradeRouteLinks nav.

        [NotMapped]
        public Region? Region => ParentLocation as Region;

        [NotMapped]
        public IEnumerable<City> Cities => SubLocations.OfType<City>();
    }
}
