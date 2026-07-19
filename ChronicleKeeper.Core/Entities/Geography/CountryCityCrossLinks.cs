using ChronicleKeeper.Core.Entities.Social;
using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Entities.Social.Economy;
using ChronicleKeeper.Core.Entities.Social.Nationality;
using ChronicleKeeper.Core.Entities.Social.Politics;
using ChronicleKeeper.Core.Entities.Social.Religions;

namespace ChronicleKeeper.Core.Entities.Geography
{
    // Country/City many-to-many cross-links. Composite-PK join POCOs (not LoreEntity), Country/City
    // is the owning side (holds the nav collection); the target entity gets no reverse nav, matching
    // the asymmetric precedent for Nation/Location/etc. (CultureNation, RegionSapientSpecies).
    // All FKs Cascade both ways — join tables share no cascading ancestor (WorldId is always Restrict),
    // and each pair targets a distinct table so no multiple-cascade-path convergence.

    // ---- Country-owned ----

    /// <summary>Join: Country ↔ Industry (composite PK, not a LoreEntity).</summary>
    public class CountryIndustry
    {
        public int CountryId { get; set; }
        public virtual Country? Country { get; set; }

        public int IndustryId { get; set; }
        public virtual Industry? Industry { get; set; }
    }

    /// <summary>Join: Country ↔ Corporation (composite PK, not a LoreEntity).</summary>
    public class CountryCorporation
    {
        public int CountryId { get; set; }
        public virtual Country? Country { get; set; }

        public int CorporationId { get; set; }
        public virtual Corporation? Corporation { get; set; }
    }

    /// <summary>Join: Country ↔ Guild (composite PK, not a LoreEntity).</summary>
    public class CountryGuild
    {
        public int CountryId { get; set; }
        public virtual Country? Country { get; set; }

        public int GuildId { get; set; }
        public virtual Guild? Guild { get; set; }
    }

    /// <summary>Join: Country ↔ PoliticalParty (composite PK, not a LoreEntity).</summary>
    public class CountryPoliticalParty
    {
        public int CountryId { get; set; }
        public virtual Country? Country { get; set; }

        public int PoliticalPartyId { get; set; }
        public virtual PoliticalParty? PoliticalParty { get; set; }
    }

    /// <summary>Join: Country ↔ Nation (composite PK, not a LoreEntity).</summary>
    public class CountryNation
    {
        public int CountryId { get; set; }
        public virtual Country? Country { get; set; }

        public int NationId { get; set; }
        public virtual Nation? Nation { get; set; }
    }

    /// <summary>Join: Country ↔ Faction (composite PK, not a LoreEntity).</summary>
    public class CountryFaction
    {
        public int CountryId { get; set; }
        public virtual Country? Country { get; set; }

        public int FactionId { get; set; }
        public virtual Faction? Faction { get; set; }
    }

    /// <summary>Join: Country ↔ Culture (predominant cultures; composite PK, not a LoreEntity).</summary>
    public class CountryCulture
    {
        public int CountryId { get; set; }
        public virtual Country? Country { get; set; }

        public int CultureId { get; set; }
        public virtual Culture? Culture { get; set; }
    }

    /// <summary>Join: Country ↔ Religion (composite PK, not a LoreEntity).</summary>
    public class CountryReligion
    {
        public int CountryId { get; set; }
        public virtual Country? Country { get; set; }

        public int ReligionId { get; set; }
        public virtual Religion? Religion { get; set; }
    }

    // ---- City-owned ----

    /// <summary>Join: City ↔ Industry (composite PK, not a LoreEntity).</summary>
    public class CityIndustry
    {
        public int CityId { get; set; }
        public virtual City? City { get; set; }

        public int IndustryId { get; set; }
        public virtual Industry? Industry { get; set; }
    }

    /// <summary>Join: City ↔ Corporation (composite PK, not a LoreEntity).</summary>
    public class CityCorporation
    {
        public int CityId { get; set; }
        public virtual City? City { get; set; }

        public int CorporationId { get; set; }
        public virtual Corporation? Corporation { get; set; }
    }

    /// <summary>Join: City ↔ Guild (composite PK, not a LoreEntity).</summary>
    public class CityGuild
    {
        public int CityId { get; set; }
        public virtual City? City { get; set; }

        public int GuildId { get; set; }
        public virtual Guild? Guild { get; set; }
    }

    /// <summary>Join: City ↔ CulturalInstitution (composite PK, not a LoreEntity).</summary>
    public class CityCulturalInstitution
    {
        public int CityId { get; set; }
        public virtual City? City { get; set; }

        public int CulturalInstitutionId { get; set; }
        public virtual CulturalInstitution? CulturalInstitution { get; set; }
    }

    /// <summary>Join: City ↔ PoliticalParty (composite PK, not a LoreEntity).</summary>
    public class CityPoliticalParty
    {
        public int CityId { get; set; }
        public virtual City? City { get; set; }

        public int PoliticalPartyId { get; set; }
        public virtual PoliticalParty? PoliticalParty { get; set; }
    }

    /// <summary>Join: City ↔ Culture (predominant cultures; composite PK, not a LoreEntity).</summary>
    public class CityCulture
    {
        public int CityId { get; set; }
        public virtual City? City { get; set; }

        public int CultureId { get; set; }
        public virtual Culture? Culture { get; set; }
    }

    /// <summary>Join: City ↔ Nation (composite PK, not a LoreEntity).</summary>
    public class CityNation
    {
        public int CityId { get; set; }
        public virtual City? City { get; set; }

        public int NationId { get; set; }
        public virtual Nation? Nation { get; set; }
    }

    /// <summary>Join: City ↔ Religion (composite PK, not a LoreEntity).</summary>
    public class CityReligion
    {
        public int CityId { get; set; }
        public virtual City? City { get; set; }

        public int ReligionId { get; set; }
        public virtual Religion? Religion { get; set; }
    }

    // ---- Country/City ↔ economy-system cross-links (added alongside the society links) ----

    /// <summary>Join: Country ↔ Currency (currencies used in the country).</summary>
    public class CountryCurrency
    {
        public int CountryId { get; set; }
        public virtual Country? Country { get; set; }

        public int CurrencyId { get; set; }
        public virtual Currency? Currency { get; set; }
    }

    /// <summary>Join: City ↔ Currency (currencies used in the city).</summary>
    public class CityCurrency
    {
        public int CityId { get; set; }
        public virtual City? City { get; set; }

        public int CurrencyId { get; set; }
        public virtual Currency? Currency { get; set; }
    }

    /// <summary>Join: Country ↔ TaxationSystem (taxation systems in force in the country).</summary>
    public class CountryTaxationSystem
    {
        public int CountryId { get; set; }
        public virtual Country? Country { get; set; }

        public int TaxationSystemId { get; set; }
        public virtual TaxationSystem? TaxationSystem { get; set; }
    }

    /// <summary>Join: City ↔ TaxationSystem (taxation systems in force in the city).</summary>
    public class CityTaxationSystem
    {
        public int CityId { get; set; }
        public virtual City? City { get; set; }

        public int TaxationSystemId { get; set; }
        public virtual TaxationSystem? TaxationSystem { get; set; }
    }
}
