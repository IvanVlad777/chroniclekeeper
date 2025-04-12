using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Social.Economy;
using ChronicleKeeper.Core.Entities.Geography;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.Interfaces;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class TaxationSystem : ILoreEntity
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

        public double IncomeTaxRate { get; set; } // % of individual income
        public double CorporateTaxRate { get; set; } // % of business earnings
        public double TradeTariffRate { get; set; } // % on imports/exports
        public bool HasFlatTax { get; set; } // True = Flat rate, False = Progressive
        public bool HasWealthTax { get; set; } // Additional tax for high net worth individuals

        //public int EconomicSystemId { get; set; }
        //public EconomicSystem EconomicSystem { get; set; } = null!;

        public ICollection<EconomicSystem> UsedInEconomicSystems { get; set; } = new List<EconomicSystem>();

        public ICollection<Guild> TaxedGuilds { get; set; } = new List<Guild>(); // Which guilds pay tax
        public ICollection<Corporation> TaxedCorporations { get; set; } = new List<Corporation>(); // Which corporations pay tax

        //public ICollection<TradeRoute> TradeRoutesWithTariffs { get; set; } = new List<TradeRoute>();

        //public ICollection<Country> CountriesUsing { get; set; } = new List<Country>();
        //public ICollection<City> CitiesUsing { get; set; } = new List<City>();
    }
}
