using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class EconomicSystem : LoreEntity
    {
        public bool IsMarketDriven { get; set; } // True = Capitalist, False = Command Economy
        public bool HasStateControl { get; set; } // True = Government controls resources
        public bool IsFeudal { get; set; } // True = Feudal economy
        public bool AllowsCorporations { get; set; } // True if private companies are allowed
        public bool AllowsGuilds { get; set; } // True if artisan/trade guilds are part of the economy

        public int? TaxationSystemId { get; set; }
        public virtual TaxationSystem? TaxationSystem { get; set; }

        public int? BankingSystemId { get; set; }
        public virtual BankingSystem? BankingSystem { get; set; }

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        // Country/City side owns the FK (Country.EconomicSystemId / City.EconomicSystemId, WithMany()) —
        // no reverse collections here, matching the GovernmentSystem/LegalSystem precedent.
    }
}
