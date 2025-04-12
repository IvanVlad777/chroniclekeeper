using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Social.Economy;
using ChronicleKeeper.Core.Entities.Geography;
using System.ComponentModel.DataAnnotations.Schema;
using ChronicleKeeper.Core.Entities.Geography.Ecosystems;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.Interfaces;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class NaturalResource : ILoreEntity
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

        public string ResourceType { get; set; } = string.Empty; // Wood, Gold, Iron, Oil, Mana Crystals
        public double Quantity { get; set; } // Measured in tons, barrels, units
        public double MarketValue { get; set; } // Price per unit in local currency
        public bool IsRenewable { get; set; } // True = Sustainable, False = Depletable
        public bool IsStrategicResource { get; set; } // If the resource is militarily or politically crucial


        //[ForeignKey("ExtractionMethod")]
        public int ExtractionMethodId { get; set; }
        public ExtractionMethod ExtractionMethod { get; set; } = null!;

        public ICollection<Location> Locations { get; set; } = new List<Location>();
        public ICollection<TradeRoute> ExportRoutes { get; set; } = new List<TradeRoute>(); // Trade connections
    }
}
