using System.ComponentModel.DataAnnotations;
using static ChronicleKeeper.Core.Enums.ClimateEnums;

namespace ChronicleKeeper.Core.DTOs.WeatherPattern
{
    public class WeatherPatternDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int ClimateZoneId { get; set; }
        public WeatherPatternType PatternType { get; set; }
        public Frequency Frequency { get; set; }
        public WeatherEffect Effects { get; set; }
        public int? HistoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>Svijet vremenskog obrasca se izvodi iz klimatske zone — ne šalje se worldId.</summary>
    public class WeatherPatternCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int ClimateZoneId { get; set; }

        public WeatherPatternType PatternType { get; set; }
        public Frequency Frequency { get; set; }
        public WeatherEffect Effects { get; set; }

        public int? HistoryId { get; set; }
    }

    public class WeatherPatternUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public WeatherPatternType PatternType { get; set; }
        public Frequency Frequency { get; set; }
        public WeatherEffect Effects { get; set; }

        public int? HistoryId { get; set; }
    }
}
