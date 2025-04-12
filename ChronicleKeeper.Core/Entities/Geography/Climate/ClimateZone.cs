using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using static ChronicleKeeper.Core.Enums.ClimateEnums;

namespace ChronicleKeeper.Core.Entities.Geography.Climate
{
    public class ClimateZone : ILoreEntity
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

        public ICollection<ClimateDetail> Climates { get; set; } = new List<ClimateDetail>();

        public ClimateZoneType ZoneType { get; set; } // npr. tropska, suptropska, arktička
        public double AverageTemperature { get; set; } // Prosječna godišnja temperatura (°C)
        public double AverageHumidity { get; set; } // Prosječna vlažnost zraka (%)
        public double AveragePrecipitation { get; set; } // Prosječna godišnja količina padalina (mm)

        public bool HasDistinctSeasons { get; set; } // Ima li ova zona izražena godišnja doba?
        public ICollection<Season> Seasons { get; set; } = new List<Season>();
        public ICollection<Location> Locations { get; set; } = new List<Location>();
        public ICollection<WeatherPattern> TypicalWeatherPatterns { get; set; } = new List<WeatherPattern>();
    }

}
