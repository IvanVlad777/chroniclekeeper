using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class EconomicSystem : ILoreEntity
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

        public bool IsMarketDriven { get; set; } // True = Capitalist, False = Command Economy
        public bool HasStateControl { get; set; } // True = Government controls resources
        public bool IsFeudal { get; set; } // True = Feudal economy
        public bool AllowsCorporations { get; set; } // True if private companies are allowed
        public bool AllowsGuilds { get; set; } // True if artisan/trade guilds are part of the economy

        //[ForeignKey("TaxationSystem")]
        public int TaxationSystemId { get; set; }
        public TaxationSystem TaxationSystem { get; set; } = null!;

        //[ForeignKey("BankingSystem")] 
        public int BankingSystemId { get; set; }
        public BankingSystem BankingSystem { get; set; } = null!;

        public ICollection<Country> CountriesUsing { get; set; } = new List<Country>();
        public ICollection<City> CitiesUsing { get; set; } = new List<City>();


    }
}
