using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.Mythology
{
    // ==================== HolySite ====================
    public class HolySiteDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public string Significance { get; set; } = string.Empty;
        public bool IsPilgrimageDestination { get; set; }
        public int ReligionId { get; set; }
        public int? DeityId { get; set; }
        public int LocationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class HolySiteDetailsDto : HolySiteDto
    {
        public ReferenceDto? History { get; set; }
        public ReferenceDto? Religion { get; set; }
        public ReferenceDto? Deity { get; set; }
        public ReferenceDto? Location { get; set; }
    }

    public class HolySiteCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        [StringLength(1000)] public string Significance { get; set; } = string.Empty;
        public bool IsPilgrimageDestination { get; set; }
        [Required]
        public int ReligionId { get; set; }
        public int? DeityId { get; set; }
        [Required]
        public int LocationId { get; set; }
    }

    public class HolySiteUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        [StringLength(1000)] public string Significance { get; set; } = string.Empty;
        public bool IsPilgrimageDestination { get; set; }
        [Required]
        public int ReligionId { get; set; }
        public int? DeityId { get; set; }
        [Required]
        public int LocationId { get; set; }
    }

    // ==================== ReligiousText ====================
    public class ReligiousTextDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string ContentSummary { get; set; } = string.Empty;
        public int ReligionId { get; set; }
        public int? DeityId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ReligiousTextDetailsDto : ReligiousTextDto
    {
        public ReferenceDto? History { get; set; }
        public ReferenceDto? Religion { get; set; }
        public ReferenceDto? Deity { get; set; }
    }

    public class ReligiousTextCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        [StringLength(100)] public string Type { get; set; } = string.Empty;
        [StringLength(4000)] public string ContentSummary { get; set; } = string.Empty;
        [Required]
        public int ReligionId { get; set; }
        public int? DeityId { get; set; }
    }

    public class ReligiousTextUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        [StringLength(100)] public string Type { get; set; } = string.Empty;
        [StringLength(4000)] public string ContentSummary { get; set; } = string.Empty;
        [Required]
        public int ReligionId { get; set; }
        public int? DeityId { get; set; }
    }

    // ==================== ReligiousFestival ====================
    public class ReligiousFestivalDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public string? StartDate { get; set; }
        public int DurationDays { get; set; }
        public string Traditions { get; set; } = string.Empty;
        public bool IsPilgrimageEvent { get; set; }
        public int ReligionId { get; set; }
        public int? HolySiteId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ReligiousFestivalDetailsDto : ReligiousFestivalDto
    {
        public ReferenceDto? History { get; set; }
        public ReferenceDto? Religion { get; set; }
        public ReferenceDto? HolySite { get; set; }
    }

    public class ReligiousFestivalCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        [StringLength(100)] public string? StartDate { get; set; }
        [Range(0, int.MaxValue)] public int DurationDays { get; set; }
        [StringLength(1000)] public string Traditions { get; set; } = string.Empty;
        public bool IsPilgrimageEvent { get; set; }
        [Required]
        public int ReligionId { get; set; }
        public int? HolySiteId { get; set; }
    }

    public class ReligiousFestivalUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        [StringLength(100)] public string? StartDate { get; set; }
        [Range(0, int.MaxValue)] public int DurationDays { get; set; }
        [StringLength(1000)] public string Traditions { get; set; } = string.Empty;
        public bool IsPilgrimageEvent { get; set; }
        [Required]
        public int ReligionId { get; set; }
        public int? HolySiteId { get; set; }
    }
}
