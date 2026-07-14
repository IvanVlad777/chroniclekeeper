using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class TradeRoute : LoreEntity
    {
        public string RouteType { get; set; } = string.Empty; // Land, Sea, Air, Magic Portal
        public string MainGoods { get; set; } = string.Empty; // Goods exchanged

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public virtual ICollection<TradeRouteResource> ResourcesTraded { get; set; } = new List<TradeRouteResource>();
        public virtual ICollection<TradeRouteLocation> Locations { get; set; } = new List<TradeRouteLocation>();

        // Origin/Destination Country/City FKs and ConnectedCities from the dormant scaffold are
        // redundant with the Locations M:N above (a route's endpoints are part of its waypoints) — dropped.
    }
}
