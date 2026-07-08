using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Tags;
//using ChronicleKeeper.Core.Entities.Characters.Equipment;
//using ChronicleKeeper.Core.Entities.Geography.Climate;
//using ChronicleKeeper.Core.Entities.HistoryTimelines;
//using ChronicleKeeper.Core.Entities.Social.Cultures;
//using ChronicleKeeper.Core.Entities.Social.Economy;
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

        // Tags
        public virtual ICollection<LocationTag> Tags { get; set; } = new List<LocationTag>();

        //public ICollection<ClimateZone> ClimateConditions { get; set; } = new List<ClimateZone>(); // TODO: Uncomment when ClimateZone entity is revived
        //public ICollection<NaturalResource> NaturalResources { get; set; } = new List<NaturalResource>(); // TODO: Uncomment when NaturalResource entity is revived
        //public ICollection<Item> Items { get; set; } = new List<Item>(); // TODO: Uncomment when Item entity is revived
        //public ICollection<CulturalFestival> Festivals { get; set; } = new List<CulturalFestival>(); // TODO: Uncomment when CulturalFestival entity is revived
        //public ICollection<HolySite> HolySites { get; set; } = new List<HolySite>(); // TODO: Uncomment when HolySite entity is revived
        //public ICollection<TimelineEvent> TimelineEvents { get; set; } = new List<TimelineEvent>(); // TODO: Uncomment when TimelineEvent gets its Location link
        //public ICollection<TradeRoute> TradeRoutes { get; set; } = new List<TradeRoute>(); // TODO: Uncomment when TradeRoute entity is revived
    }
}
