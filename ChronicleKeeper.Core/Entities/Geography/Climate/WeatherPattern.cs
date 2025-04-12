using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ChronicleKeeper.Core.Enums.ClimateEnums;

namespace ChronicleKeeper.Core.Entities.Geography.Climate
{
    public class WeatherPattern : ILoreEntity
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

        //[ForeignKey("ClimateZone")]
        public int ClimateZoneId { get; set; }
        public ClimateZone ClimateZone { get; set; } = null!;

        public WeatherPatternType PatternType { get; set; } // npr. "Monsun", "Pustinjske oluje"
        public Frequency Frequency { get; set; } // Kako često se javlja (sezonski, rijetko, često)
        public WeatherEffect Effects { get; set; } // npr. poplave, suše, oluje
    }
}
