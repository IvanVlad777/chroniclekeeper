using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Characters.Equipment;
using ChronicleKeeper.Core.Entities.Geography.Climate;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Entities.Social.Economy;
using ChronicleKeeper.Core.Entities.Social.Religions;
using ChronicleKeeper.Core.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Geography
{
    public abstract class Location : ILoreEntity
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

        public double Area { get; set; }  // Površina u km²
        public int Population { get; set; }  // Broj stanovnika
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public ICollection<ClimateZone> ClimateConditions { get; set; } = new List<ClimateZone>();
        public ICollection<NaturalResource> NaturalResources { get; set; } = new List<NaturalResource>();
        public ICollection<Location> SubLocations { get; set; } = new List<Location>(); // Regije, gradovi, distrikti unutar lokacije
        public ICollection<Item> Items { get; set; } = new List<Item>();
        public ICollection<CulturalFestival> Festivals { get; set; } = new List<CulturalFestival>();
        public ICollection<HolySite> HolySites { get; set; } = new List<HolySite>();

        public int? ParentLocationId { get; set; }
        public Location? ParentLocation { get; set; }

        public ICollection<TimelineEvent> TimelineEvents { get; set; } = new List<TimelineEvent>();
        public ICollection<TradeRoute> TradeRoutes { get; set; } = new List<TradeRoute>();

    }
}
