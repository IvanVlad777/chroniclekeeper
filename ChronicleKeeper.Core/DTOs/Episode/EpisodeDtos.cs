using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.Episode
{
    public class EpisodeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int SeriesId { get; set; }
        public int Season { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class EpisodeCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>Svijet epizode se izvodi iz serije — ne šalje se WorldId.</summary>
        [Required]
        public int SeriesId { get; set; }

        public int Season { get; set; }
        public int Order { get; set; }
    }

    public class EpisodeUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public int Season { get; set; }
        public int Order { get; set; }
    }
}
