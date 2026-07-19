using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.Timeline
{
    public class TimelineDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class TimelineDetailsDto : TimelineDto
    {
        public ReferenceDto? History { get; set; }

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

        public int? HistoryId { get; set; }
    }

    public class TimelineUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public int? HistoryId { get; set; }
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
        public string Era { get; set; } = string.Empty;
        public string Consequences { get; set; } = string.Empty;
        public bool IsMajorEvent { get; set; }
        /// <summary>Where the event took place (optional).</summary>
        public ReferenceDto? Location { get; set; }
        /// <summary>Battle this event represents (optional).</summary>
        public ReferenceDto? Battle { get; set; }
        /// <summary>Folklore/legend this event is drawn from (optional).</summary>
        public ReferenceDto? Folklore { get; set; }
        /// <summary>Characters involved in the event.</summary>
        public List<ReferenceDto> InvolvedCharacters { get; set; } = new();
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

        /// <summary>Optional era/period label used to group events (e.g. "Third Age").</summary>
        [StringLength(100)]
        public string Era { get; set; } = string.Empty;

        [StringLength(2000)]
        public string Consequences { get; set; } = string.Empty;

        public bool IsMajorEvent { get; set; }

        /// <summary>Where the event took place (optional; must be in the timeline's world).</summary>
        public int? LocationId { get; set; }

        /// <summary>Battle this event represents (optional; must be in the timeline's world).</summary>
        public int? BattleId { get; set; }

        /// <summary>Folklore/legend this event is drawn from (optional; must be in the timeline's world).</summary>
        public int? FolkloreId { get; set; }

        /// <summary>Characters involved (must be in the timeline's world).</summary>
        public List<int> InvolvedCharacterIds { get; set; } = new();
    }

    public class TimelineEventUpdateDto : TimelineEventCreateDto { }
}
