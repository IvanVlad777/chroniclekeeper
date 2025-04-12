using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Social.Politics
{
    public class PoliticalIdeology : ILoreEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public virtual History? History { get; set; }

        public bool IsAuthoritarian { get; set; } // True if it promotes authoritarian rule
        public bool IsSocialist { get; set; } // True if it aligns with socialist principles
        public bool IsLiberal { get; set; } // True if it promotes individual freedoms
        public bool IsRadical { get; set; } // Ako je ekstremna ideologija (fašizam, komunizam, anarhizam)
        public bool IsMilitaristic { get; set; } // Ako se fokusira na vojnu ekspanziju

        // ✅ Ekonomska orijentacija
        public bool SupportsFreeMarket { get; set; } // Ako podržava kapitalizam
        public bool SupportsPlannedEconomy { get; set; } // Ako podržava državnu kontrolu ekonomije

        public ICollection<PoliticalParty> AffiliatedPoliticalParties { get; set; } = new List<PoliticalParty>();
        public ICollection<GovernmentSystem> AffiliatedGovernmentSystems { get; set; } = new List<GovernmentSystem>();


    }
}
