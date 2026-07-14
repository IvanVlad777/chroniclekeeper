using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class TaxationSystem : LoreEntity
    {
        public double IncomeTaxRate { get; set; } // % of individual income
        public double CorporateTaxRate { get; set; } // % of business earnings
        public double TradeTariffRate { get; set; } // % on imports/exports
        public bool HasFlatTax { get; set; } // True = Flat rate, False = Progressive
        public bool HasWealthTax { get; set; } // Additional tax for high net worth individuals

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public virtual ICollection<EconomicSystem> UsedInEconomicSystems { get; set; } = new List<EconomicSystem>();
        public virtual ICollection<Guild> TaxedGuilds { get; set; } = new List<Guild>(); // Which guilds pay tax
        public virtual ICollection<Corporation> TaxedCorporations { get; set; } = new List<Corporation>(); // Which corporations pay tax

        //public ICollection<TradeRoute> TradeRoutesWithTariffs { get; set; } = new List<TradeRoute>(); // TODO: Many-to-many cross-link, needs join entity — deferred, same pattern as Culture<->Nation
        //public ICollection<Country> CountriesUsing { get; set; } = new List<Country>(); // TODO: Deferred with the Country/City M:N bucket
        //public ICollection<City> CitiesUsing { get; set; } = new List<City>(); // TODO: Deferred with the Country/City M:N bucket
    }
}
