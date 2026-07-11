using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.WeatherPattern;
using static ChronicleKeeper.Core.Enums.ClimateEnums;

namespace ChronicleKeeper.Core.DTOs.ClimateZone
{
    public class ClimateZoneDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public ClimateZoneType ZoneType { get; set; }
        public double AverageTemperature { get; set; }
        public double AverageHumidity { get; set; }
        public double AveragePrecipitation { get; set; }
        public bool HasDistinctSeasons { get; set; }
        public int? HistoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ClimateZoneDetailsDto : ClimateZoneDto
    {
        public ReferenceDto? History { get; set; }
        public List<ReferenceDto> Climates { get; set; } = new();
        public List<ReferenceDto> Seasons { get; set; } = new();
        public List<ReferenceDto> Locations { get; set; } = new();
        public List<WeatherPatternDto> WeatherPatterns { get; set; } = new();
    }

    public class ClimateZoneCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        public ClimateZoneType ZoneType { get; set; }

        [Range(-100, 100)]
        public double AverageTemperature { get; set; }
        [Range(0, 100)]
        public double AverageHumidity { get; set; }
        [Range(0, 10000)]
        public double AveragePrecipitation { get; set; }

        public bool HasDistinctSeasons { get; set; }

        public int? HistoryId { get; set; }
    }

    public class ClimateZoneUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public ClimateZoneType ZoneType { get; set; }

        [Range(-100, 100)]
        public double AverageTemperature { get; set; }
        [Range(0, 100)]
        public double AverageHumidity { get; set; }
        [Range(0, 10000)]
        public double AveragePrecipitation { get; set; }

        public bool HasDistinctSeasons { get; set; }

        public int? HistoryId { get; set; }
    }
}
