using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Tags;
using ChronicleKeeper.Core.Entities.Characters.Equipment;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Entities.Social.Economy;
//using ChronicleKeeper.Core.Entities.Geography.Climate;
//using ChronicleKeeper.Core.Entities.Social.Cultures;
//using ChronicleKeeper.Core.Entities.Social.Religions;
using static ChronicleKeeper.Core.Enums.LoreEnums;

namespace ChronicleKeeper.Core.Entities.Geography
{
    public class Location : LoreEntity
    {
        public LocationType Type { get; set; }

        public double? Area { get; set; }  // Površina u km²
        public int? Population { get; set; }  // Broj stanovnika
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Hierarchy (self-referencing): regije, gradovi, distrikti unutar lokacije
        public int? ParentLocationId { get; set; }
        public virtual Location? ParentLocation { get; set; }
        public virtual ICollection<Location> SubLocations { get; set; } = new List<Location>();

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        // Tags
        public virtual ICollection<LocationTag> Tags { get; set; } = new List<LocationTag>();

        // NOTE: relation to ClimateZone already exists via the LocationClimateZone join entity (ClimateZone owns the nav) — target side gets no nav per convention.
        // NOTE: relation to NaturalResource already exists via the NaturalResourceLocation join entity (NaturalResource owns the nav) — target side gets no nav per convention.
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
        public virtual ICollection<School> Schools { get; set; } = new List<School>();
        //public ICollection<CulturalFestival> Festivals { get; set; } = new List<CulturalFestival>(); // TODO: Uncomment when CulturalFestival entity is revived
        //public ICollection<HolySite> HolySites { get; set; } = new List<HolySite>(); // TODO: Uncomment when HolySite entity is revived
        //public ICollection<TimelineEvent> TimelineEvents { get; set; } = new List<TimelineEvent>(); // TODO: Uncomment when TimelineEvent gets its Location link
        // Reverse read-only nav for the TradeRoute-owned TradeRouteLocation join — surfaced on
        // Country/City detail as a read list (the TradeRoute owns the write side).
        public virtual ICollection<TradeRouteLocation> TradeRouteLinks { get; set; } = new List<TradeRouteLocation>();
    }
}
