using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using static ChronicleKeeper.Core.Enums.ClimateEnums;

namespace ChronicleKeeper.Core.Entities.Geography.Climate
{
    public class ClimateZone : LoreEntity
    {
        public ClimateZoneType ZoneType { get; set; } // npr. tropska, suptropska, arktička
        public double AverageTemperature { get; set; } // Prosječna godišnja temperatura (°C)
        public double AverageHumidity { get; set; } // Prosječna vlažnost zraka (%)
        public double AveragePrecipitation { get; set; } // Prosječna godišnja količina padalina (mm)

        public bool HasDistinctSeasons { get; set; } // Ima li ova zona izražena godišnja doba?

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public virtual ICollection<ClimateZoneDetail> Climates { get; set; } = new List<ClimateZoneDetail>();
        public virtual ICollection<ClimateZoneSeason> Seasons { get; set; } = new List<ClimateZoneSeason>();
        public virtual ICollection<LocationClimateZone> Locations { get; set; } = new List<LocationClimateZone>();
        public virtual ICollection<WeatherPattern> TypicalWeatherPatterns { get; set; } = new List<WeatherPattern>();
    }
}
