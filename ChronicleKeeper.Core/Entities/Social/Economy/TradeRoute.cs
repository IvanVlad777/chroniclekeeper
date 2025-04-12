using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class TradeRoute : ILoreEntity
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

        public ICollection<NaturalResource> ResourcesTraded { get; set; } = new List<NaturalResource>();
        public ICollection<Location> Locations { get; set; } = new List<Location>();


        //public string RouteType { get; set; } = string.Empty; // Land, Sea, Air, Magic Portal
        //public string MainGoods { get; set; } = string.Empty; // Goods exchanged

        //[ForeignKey("OriginCountry")]
        //public int? OriginCountryId { get; set; }

        //[ForeignKey("DestinationCountry")]
        //public int? DestinationCountryId { get; set; }

        //[ForeignKey("OriginCity")]
        //public int? OriginCityId { get; set; }

        //[ForeignKey("DestinationCity")]
        //public int? DestinationCityId { get; set; }

        //public ICollection<City> ConnectedCities { get; set; } = new List<City>();
    }
}
