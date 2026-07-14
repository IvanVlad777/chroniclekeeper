using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class BankingSystem : LoreEntity
    {
        public string SystemType { get; set; } = string.Empty; // Central Bank, Private Banking, Merchant Banking
        public double InterestRate { get; set; } // Standard interest rate
        public bool AllowsLoans { get; set; } // If lending is part of the system
        public bool HasStateControl { get; set; } // If the government regulates banking
        public bool SupportsForeignInvestment { get; set; } // If foreign money is allowed

        public int? CurrencyId { get; set; } // The main currency of this banking system
        public virtual Currency? Currency { get; set; }

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        //public int? CountryId { get; set; } // TODO: Uncomment when a national-level link is designed — deferred with the Country/City M:N bucket
        //public Country? Country { get; set; }
        //public int? CityId { get; set; } // TODO: Uncomment when a city-level link is designed — deferred with the Country/City M:N bucket
        //public City? City { get; set; }

        public virtual ICollection<EconomicSystem> UsedInEconomicSystems { get; set; } = new List<EconomicSystem>();
        public virtual ICollection<Corporation> FinancialInstitutions { get; set; } = new List<Corporation>(); // Banks & financial companies
    }
}
