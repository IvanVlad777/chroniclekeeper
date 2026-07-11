using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.Content
{
    /// <summary>
    /// Flat DTO for the Article/Book/Comic/Movie/Series TPH family. Only the fields relevant to
    /// <see cref="Type"/> are populated — every other type-specific field stays null.
    /// </summary>
    public class ContentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }

        /// <summary>"Article" | "Book" | "Comic" | "Movie" | "Series" — mirrors the "ContentType" TPH discriminator.</summary>
        public string Type { get; set; } = string.Empty;

        // Article
        public string? Source { get; set; }
        public DateTime? PublishDate { get; set; }

        // Book / Comic
        public string? Author { get; set; }

        // Book / Movie (only one populated per row, depending on Type)
        public DateTime? ReleaseDate { get; set; }

        // Comic
        public int? IssueNumber { get; set; }

        // Movie
        public string? Director { get; set; }
        public int? DurationMinutes { get; set; }
        public int? PrequelId { get; set; }

        // Series
        public string? Creator { get; set; }
        public int? Seasons { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ContentDetailsDto : ContentDto
    {
        /// <summary>Populated only when Type == "Book".</summary>
        public List<ReferenceDto> Chapters { get; set; } = new();

        /// <summary>Populated only when Type == "Series".</summary>
        public List<ReferenceDto> Episodes { get; set; } = new();

        /// <summary>Populated only when Type == "Movie".</summary>
        public ReferenceDto? Prequel { get; set; }

        /// <summary>Populated only when Type == "Movie".</summary>
        public List<ReferenceDto> Sequels { get; set; } = new();

        public List<ContentReferenceEntryDto> References { get; set; } = new();
    }

    /// <summary>One row from this Content's own References collection, resolved to whichever
    /// world-entity side (Character/Location/Faction/Nation) is set on it.</summary>
    public class ContentReferenceEntryDto
    {
        public int Id { get; set; }
        public string Note { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public int EntityId { get; set; }
        public string EntityName { get; set; } = string.Empty;
    }

    public class ContentCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        /// <summary>"Article" | "Book" | "Comic" | "Movie" | "Series" — validated against the known TPH discriminator values.</summary>
        [Required]
        public string Type { get; set; } = string.Empty;

        // Article
        [StringLength(200)]
        public string? Source { get; set; }
        public DateTime? PublishDate { get; set; }

        // Book / Comic
        [StringLength(100)]
        public string? Author { get; set; }

        // Book / Movie
        public DateTime? ReleaseDate { get; set; }

        // Comic
        public int? IssueNumber { get; set; }

        // Movie
        [StringLength(100)]
        public string? Director { get; set; }
        public int? DurationMinutes { get; set; }
        public int? PrequelId { get; set; }

        // Series
        [StringLength(100)]
        public string? Creator { get; set; }
        public int? Seasons { get; set; }
    }

    /// <summary>Same as <see cref="ContentCreateDto"/> minus WorldId and Type — a Content's concrete
    /// type can't change after creation (same "can't change parent" convention as JobRankUpdateDto).</summary>
    public class ContentUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Source { get; set; }
        public DateTime? PublishDate { get; set; }

        [StringLength(100)]
        public string? Author { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int? IssueNumber { get; set; }

        [StringLength(100)]
        public string? Director { get; set; }
        public int? DurationMinutes { get; set; }
        public int? PrequelId { get; set; }

        [StringLength(100)]
        public string? Creator { get; set; }
        public int? Seasons { get; set; }
    }
}
