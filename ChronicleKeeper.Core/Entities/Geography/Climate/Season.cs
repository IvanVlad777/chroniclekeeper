using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Geography.Climate
{
    public class Season : LoreEntity
    {
        [Range(1, 10000)]
        public int DurationInDays { get; set; } // Trajanje sezone u danima
        [Range(-100, 100)]
        public double TypicalTemperature { get; set; } // Prosječna temperatura tijekom sezone
        [Range(0, 10000)]
        public double TypicalPrecipitation { get; set; } // Prosječne padaline u sezoni

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public virtual ICollection<ClimateZoneSeason> ClimateZones { get; set; } = new List<ClimateZoneSeason>();
    }
}
