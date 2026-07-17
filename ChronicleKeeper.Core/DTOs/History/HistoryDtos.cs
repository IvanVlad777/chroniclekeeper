using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.History
{
    public class HistoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public string Summary { get; set; } = string.Empty;
        public bool IsOfficial { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class HistoryDetailsDto : HistoryDto
    {
        /// <summary>Timelines belonging to this history, with summary counts for the hub cards.</summary>
        public List<HistoryTimelineDto> Timelines { get; set; } = new();

        /// <summary>Entities (Character/Location/Faction/Nation) that point at this history.</summary>
        public List<HistoryLinkDto> LinkedEntities { get; set; } = new();
    }

    /// <summary>A timeline card on the history hub: name + event/major counts + span (first–last event date).</summary>
    public class HistoryTimelineDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int EventCount { get; set; }
        public int MajorEventCount { get; set; }
        /// <summary>Date of the first event (by SortOrder); empty when the timeline has no events.</summary>
        public string FirstDate { get; set; } = string.Empty;
        /// <summary>Date of the last event (by SortOrder); empty when the timeline has no events.</summary>
        public string LastDate { get; set; } = string.Empty;
    }

    /// <summary>An entity that links to a history — its type discriminator, id and name.</summary>
    public class HistoryLinkDto
    {
        /// <summary>"Character" | "Location" | "Faction" | "Nation".</summary>
        public string Type { get; set; } = string.Empty;
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>Entity types that can carry a History link (set from the entity's detail block).</summary>
    public enum HistoryLinkTargetType
    {
        Character,
        Location,
        Faction,
        Nation
    }

    public class HistoryCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [StringLength(4000, ErrorMessage = "Summary cannot exceed 4000 characters")]
        public string Summary { get; set; } = string.Empty;

        public bool IsOfficial { get; set; }
    }

    public class HistoryUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Summary cannot exceed 4000 characters")]
        public string Summary { get; set; } = string.Empty;

        public bool IsOfficial { get; set; }
    }
}
