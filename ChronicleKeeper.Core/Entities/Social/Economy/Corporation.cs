using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Social.Economy;
using ChronicleKeeper.Core.Entities.Geography;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ChronicleKeeper.Core.Entities.Professions;
using ChronicleKeeper.Core.Interfaces;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class Corporation : ILoreEntity
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

        public string IndustrySector { get; set; } = string.Empty; // Banking, Manufacturing, Mining, Retail
        public double Revenue { get; set; } // Annual revenue in native currency
        public int NumberOfEmployees { get; set; } // Workforce size
        public bool IsPubliclyTraded { get; set; } // True if it has shareholders
        public bool IsStateOwned { get; set; } // True if a government-controlled enterprise

        //[ForeignKey("Industry")]
        public int? IndustryId { get; set; } // Industry classification
        public Industry? Industry { get; set; }

        //public int EconomicSystemId { get; set; } // Economic regulations applied
        //public EconomicSystem EconomicSystem { get; set; } = null!;

        public int? TaxationSystemId { get; set; }
        public TaxationSystem? TaxationSystem { get; set; }

        public int? BankingSystemId { get; set; } // If this corporation relies on a financial system
        public BankingSystem? BankingSystem { get; set; }

        public ICollection<Country> PresentInCountries { get; set; } = new List<Country>();
        public ICollection<City> PresentInCities { get; set; } = new List<City>();
        public ICollection<Faction> Factions { get; set; } = new List<Faction>();

        public ICollection<Profession> MemberProfessions { get; set; } = new List<Profession>(); // Jobs in the guild

        public ICollection<Apprenticeship> Apprenticeships { get; set; } = new List<Apprenticeship>(); // ✅ Training opportunities
        public ICollection<CorporateLeadership> Leadership { get; set; } = new List<CorporateLeadership>(); // CEOs, CFOs, etc.
        //public ICollection<Corporation> Subsidiaries { get; set; } = new List<Corporation>(); // Corporations owned by this one
    }
}
