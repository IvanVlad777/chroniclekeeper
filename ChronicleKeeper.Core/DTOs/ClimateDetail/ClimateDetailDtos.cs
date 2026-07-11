using System.ComponentModel.DataAnnotations;
using static ChronicleKeeper.Core.Enums.ClimateEnums;

namespace ChronicleKeeper.Core.DTOs.ClimateDetail
{
    public class ClimateDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public double AverageTemperature { get; set; }
        public double Humidity { get; set; }
        public double Precipitation { get; set; }
        public double WindSpeed { get; set; }
        public WindDirection WindDirection { get; set; }
        public bool IsExtremeClimate { get; set; }
        public NotableWeatherPhenomena NotableWeatherPhenomena { get; set; }
        public int? HistoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ClimateDetailCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [Range(-100, 100)]
        public double AverageTemperature { get; set; }
        [Range(0, 100)]
        public double Humidity { get; set; }
        [Range(0, 10000)]
        public double Precipitation { get; set; }
        [Range(0, 500)]
        public double WindSpeed { get; set; }
        public WindDirection WindDirection { get; set; }

        public bool IsExtremeClimate { get; set; }
        public NotableWeatherPhenomena NotableWeatherPhenomena { get; set; }

        public int? HistoryId { get; set; }
    }

    public class ClimateDetailUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Range(-100, 100)]
        public double AverageTemperature { get; set; }
        [Range(0, 100)]
        public double Humidity { get; set; }
        [Range(0, 10000)]
        public double Precipitation { get; set; }
        [Range(0, 500)]
        public double WindSpeed { get; set; }
        public WindDirection WindDirection { get; set; }

        public bool IsExtremeClimate { get; set; }
        public NotableWeatherPhenomena NotableWeatherPhenomena { get; set; }

        public int? HistoryId { get; set; }
    }
}
