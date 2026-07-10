using ChronicleKeeper.Core.Entities.Base;

namespace ChronicleKeeper.Core.Entities.Social.Politics
{
    public class PoliticalIdeology : LoreEntity
    {
        public bool IsAuthoritarian { get; set; } // Promotes authoritarian rule
        public bool IsSocialist { get; set; } // Aligns with socialist principles
        public bool IsLiberal { get; set; } // Promotes individual freedoms
        public bool IsRadical { get; set; } // Extreme ideology (fascism, communism, anarchism...)
        public bool IsMilitaristic { get; set; } // Focused on military expansion

        // Economic orientation
        public bool SupportsFreeMarket { get; set; } // Supports capitalism
        public bool SupportsPlannedEconomy { get; set; } // Supports state-controlled economy

        public ICollection<PoliticalParty> AffiliatedPoliticalParties { get; set; } = new List<PoliticalParty>();
        public ICollection<GovernmentSystem> AffiliatedGovernmentSystems { get; set; } = new List<GovernmentSystem>();
    }
}
