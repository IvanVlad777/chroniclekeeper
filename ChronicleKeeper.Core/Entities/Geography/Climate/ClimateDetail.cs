using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using System.ComponentModel.DataAnnotations;
using static ChronicleKeeper.Core.Enums.ClimateEnums;

namespace ChronicleKeeper.Core.Entities.Geography.Climate
{
    public class ClimateDetail : LoreEntity
    {
        [Range(-100, 100)]
        public double AverageTemperature { get; set; } // Prosječna temperatura (°C)
        [Range(0, 100)]
        public double Humidity { get; set; } // Vlažnost zraka (%)
        [Range(0, 10000)]
        public double Precipitation { get; set; } // Godišnja količina padalina (mm)
        [Range(0, 500)]
        public double WindSpeed { get; set; } // Prosječna brzina vjetra (km/h)
        public WindDirection WindDirection { get; set; } // Dominantni smjer vjetra

        public bool IsExtremeClimate { get; set; } // Označava li ovo ekstremne vremenske uvjete?
        public NotableWeatherPhenomena NotableWeatherPhenomena { get; set; } // npr. tornada, uragani, suše, magle

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public virtual ICollection<ClimateZoneDetail> ClimateZones { get; set; } = new List<ClimateZoneDetail>();
    }
}
