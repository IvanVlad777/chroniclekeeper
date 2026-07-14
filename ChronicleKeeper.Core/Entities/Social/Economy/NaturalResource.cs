using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class NaturalResource : LoreEntity
    {
        public string ResourceType { get; set; } = string.Empty; // Wood, Gold, Iron, Oil, Mana Crystals
        public double Quantity { get; set; } // Measured in tons, barrels, units
        public double MarketValue { get; set; } // Price per unit in local currency
        public bool IsRenewable { get; set; } // True = Sustainable, False = Depletable
        public bool IsStrategicResource { get; set; } // If the resource is militarily or politically crucial

        public int? ExtractionMethodId { get; set; }
        public virtual ExtractionMethod? ExtractionMethod { get; set; }

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public virtual ICollection<NaturalResourceLocation> Locations { get; set; } = new List<NaturalResourceLocation>();
        public virtual ICollection<TradeRouteResource> ExportRoutes { get; set; } = new List<TradeRouteResource>(); // Trade connections
    }
}
