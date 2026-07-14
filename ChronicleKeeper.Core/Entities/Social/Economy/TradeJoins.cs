using ChronicleKeeper.Core.Entities.Geography;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    /// <summary>Join: TradeRoute ↔ Location (composite PK, not a LoreEntity).</summary>
    public class TradeRouteLocation
    {
        public int TradeRouteId { get; set; }
        public virtual TradeRoute? TradeRoute { get; set; }

        public int LocationId { get; set; }
        public virtual Location? Location { get; set; }
    }

    /// <summary>Join: TradeRoute ↔ NaturalResource (composite PK, not a LoreEntity). Both sides expose a nav.</summary>
    public class TradeRouteResource
    {
        public int TradeRouteId { get; set; }
        public virtual TradeRoute? TradeRoute { get; set; }

        public int NaturalResourceId { get; set; }
        public virtual NaturalResource? NaturalResource { get; set; }
    }

    /// <summary>Join: NaturalResource ↔ Location (composite PK, not a LoreEntity).</summary>
    public class NaturalResourceLocation
    {
        public int NaturalResourceId { get; set; }
        public virtual NaturalResource? NaturalResource { get; set; }

        public int LocationId { get; set; }
        public virtual Location? Location { get; set; }
    }
}
