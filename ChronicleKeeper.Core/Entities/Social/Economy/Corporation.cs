using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Professions;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class Corporation : LoreEntity
    {
        public string IndustrySector { get; set; } = string.Empty; // Banking, Manufacturing, Mining, Retail
        public double Revenue { get; set; } // Annual revenue in native currency
        public int NumberOfEmployees { get; set; } // Workforce size
        public bool IsPubliclyTraded { get; set; } // True if it has shareholders
        public bool IsStateOwned { get; set; } // True if a government-controlled enterprise

        public int? IndustryId { get; set; } // Industry classification
        public virtual Industry? Industry { get; set; }

        public int? TaxationSystemId { get; set; }
        public virtual TaxationSystem? TaxationSystem { get; set; }

        public int? BankingSystemId { get; set; } // If this corporation relies on a financial system
        public virtual BankingSystem? BankingSystem { get; set; }

        public int? ParentCorporationId { get; set; } // Owning corporation, if a subsidiary
        public virtual Corporation? ParentCorporation { get; set; }
        public virtual ICollection<Corporation> Subsidiaries { get; set; } = new List<Corporation>();

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public virtual ICollection<CorporateLeadership> Leadership { get; set; } = new List<CorporateLeadership>(); // CEOs, CFOs, etc.
        public virtual ICollection<Apprenticeship> Apprenticeships { get; set; } = new List<Apprenticeship>(); // Training opportunities

        public virtual ICollection<CorporationFaction> Factions { get; set; } = new List<CorporationFaction>();
        public virtual ICollection<CorporationProfession> MemberProfessions { get; set; } = new List<CorporationProfession>(); // Jobs in the corporation

        //public ICollection<Country> PresentInCountries { get; set; } = new List<Country>(); // TODO: Deferred with the Country/City M:N bucket
        //public ICollection<City> PresentInCities { get; set; } = new List<City>(); // TODO: Deferred with the Country/City M:N bucket
    }
}
