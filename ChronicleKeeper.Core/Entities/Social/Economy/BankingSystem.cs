using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Religions;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class BankingSystem : ILoreEntity
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
        public string SystemType { get; set; } = string.Empty; // Central Bank, Private Banking, Merchant Banking
        public double InterestRate { get; set; } // Standard interest rate
        public bool AllowsLoans { get; set; } // If lending is part of the system
        public bool HasStateControl { get; set; } // If the government regulates banking
        public bool SupportsForeignInvestment { get; set; } // If foreign money is allowed

        //[ForeignKey("Currency")]
        public int CurrencyId { get; set; } // The main currency of this banking system
        public Currency Currency { get; set; } = null!;


        //[ForeignKey("Country")]
        //public int? CountryId { get; set; } // If the system operates at a national level
        //public Country? Country { get; set; } = null!;

        //[ForeignKey("City")]
        //public int? CityId { get; set; } // If the system is city-specific
        //public City? City { get; set; }
        public ICollection<EconomicSystem> UsedInEconomicSystems { get; set; } = new List<EconomicSystem>();
        public ICollection<Corporation> FinancialInstitutions { get; set; } = new List<Corporation>(); // Banks & financial companies
    }
}
