using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class Currency : ILoreEntity
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

        public string Symbol { get; set; } = string.Empty; // $, €, ¥, etc.
        public double ExchangeRate { get; set; } // Compared to a base currency
        public string BackingType { get; set; } = string.Empty; // Gold-backed, Fiat, Magic-backed

        //public int BankingSystemId { get; set; }
        //public BankingSystem BankingSystem { get; set; } = null!;

        //public int EconomicSystemId { get; set; }
        //public EconomicSystem EconomicSystem { get; set; } = null!;

        //public ICollection<Country> UsedInCountries { get; set; } = new List<Country>();
    }
}
