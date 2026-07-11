using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using static ChronicleKeeper.Core.Enums.ClimateEnums;

namespace ChronicleKeeper.Core.Entities.Geography.Climate
{
    public class WeatherPattern : LoreEntity
    {
        public int ClimateZoneId { get; set; }
        public virtual ClimateZone ClimateZone { get; set; } = null!;

        public WeatherPatternType PatternType { get; set; } // npr. "Monsun", "Pustinjske oluje"
        public Frequency Frequency { get; set; } // Kako često se javlja (sezonski, rijetko, često)
        public WeatherEffect Effects { get; set; } // npr. poplave, suše, oluje

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }
    }
}
