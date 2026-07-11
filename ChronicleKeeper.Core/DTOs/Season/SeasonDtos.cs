using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.Season
{
    public class SeasonDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int DurationInDays { get; set; }
        public double TypicalTemperature { get; set; }
        public double TypicalPrecipitation { get; set; }
        public int? HistoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class SeasonCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [Range(1, 10000)]
        public int DurationInDays { get; set; }
        [Range(-100, 100)]
        public double TypicalTemperature { get; set; }
        [Range(0, 10000)]
        public double TypicalPrecipitation { get; set; }

        public int? HistoryId { get; set; }
    }

    public class SeasonUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Range(1, 10000)]
        public int DurationInDays { get; set; }
        [Range(-100, 100)]
        public double TypicalTemperature { get; set; }
        [Range(0, 10000)]
        public double TypicalPrecipitation { get; set; }

        public int? HistoryId { get; set; }
    }
}
