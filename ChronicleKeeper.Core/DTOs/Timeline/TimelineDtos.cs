using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.Timeline
{
    public class TimelineDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class TimelineDetailsDto : TimelineDto
    {
        /// <summary>Eventi poredani po SortOrder.</summary>
        public List<TimelineEventDto> Events { get; set; } = new();
    }

    public class TimelineCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }
    }

    public class TimelineUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;
    }

    public class TimelineEventDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int TimelineId { get; set; }
        public string Date { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public string Consequences { get; set; } = string.Empty;
        public bool IsMajorEvent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    // Svijet eventa se izvodi iz timelinea — ne šalje se WorldId
    public class TimelineEventCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>Slobodan in-world datum, npr. "Godina 512, Treće doba".</summary>
        [StringLength(100)]
        public string Date { get; set; } = string.Empty;

        public int SortOrder { get; set; }

        [StringLength(2000)]
        public string Consequences { get; set; } = string.Empty;

        public bool IsMajorEvent { get; set; }
    }

    public class TimelineEventUpdateDto : TimelineEventCreateDto { }
}
