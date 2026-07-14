using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class Currency : LoreEntity
    {
        public string Symbol { get; set; } = string.Empty; // $, €, ¥, etc.
        public double ExchangeRate { get; set; } // Compared to a base currency
        public string BackingType { get; set; } = string.Empty; // Gold-backed, Fiat, Magic-backed

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        // Relation to BankingSystem/EconomicSystem already exists via BankingSystem.CurrencyId
        // (and EconomicSystem -> BankingSystem) — no reverse FK here.

        //public ICollection<Country> UsedInCountries { get; set; } = new List<Country>(); // TODO: Many-to-many cross-link, needs join entity — deferred, same pattern as Culture<->Nation
    }
}
