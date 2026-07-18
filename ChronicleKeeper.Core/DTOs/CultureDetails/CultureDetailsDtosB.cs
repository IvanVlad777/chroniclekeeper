using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.CultureDetails
{
    // ---------------- ArchitectureStyle ----------------
    public class ArchitectureStyleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public string MaterialsUsed { get; set; } = string.Empty;
        public string DesignFeatures { get; set; } = string.Empty;
        public bool IsFortified { get; set; }
        public int CultureId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ArchitectureStyleDetailsDto : ArchitectureStyleDto
    {
        public ReferenceDto? History { get; set; }
        public ReferenceDto? Culture { get; set; }
        public List<ReferenceDto> TypicalLocations { get; set; } = new();
    }

    public class ArchitectureStyleCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        [StringLength(500)] public string MaterialsUsed { get; set; } = string.Empty;
        [StringLength(500)] public string DesignFeatures { get; set; } = string.Empty;
        public bool IsFortified { get; set; }
        [Required]
        public int CultureId { get; set; }
    }

    public class ArchitectureStyleUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        [StringLength(500)] public string MaterialsUsed { get; set; } = string.Empty;
        [StringLength(500)] public string DesignFeatures { get; set; } = string.Empty;
        public bool IsFortified { get; set; }
        [Required]
        public int CultureId { get; set; }
    }

    // ---------------- Folklore ----------------
    public class FolkloreDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public string Story { get; set; } = string.Empty;
        public string Moral { get; set; } = string.Empty;
        public bool IsHistorical { get; set; }
        public int CultureId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class FolkloreDetailsDto : FolkloreDto
    {
        public ReferenceDto? History { get; set; }
        public ReferenceDto? Culture { get; set; }
        public List<ReferenceDto> RelatedEvents { get; set; } = new();
        public List<ReferenceDto> OriginatedFromSpecies { get; set; } = new();
    }

    public class FolkloreCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        [StringLength(2000)] public string Story { get; set; } = string.Empty;
        [StringLength(500)] public string Moral { get; set; } = string.Empty;
        public bool IsHistorical { get; set; }
        [Required]
        public int CultureId { get; set; }
    }

    public class FolkloreUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        [StringLength(2000)] public string Story { get; set; } = string.Empty;
        [StringLength(500)] public string Moral { get; set; } = string.Empty;
        public bool IsHistorical { get; set; }
        [Required]
        public int CultureId { get; set; }
    }

    // ---------------- Myth ----------------
    public class MythDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public string CreationStory { get; set; } = string.Empty;
        public string Symbolism { get; set; } = string.Empty;
        public bool HasReligiousConnections { get; set; }
        public int CultureId { get; set; }
        public int? ReligionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class MythDetailsDto : MythDto
    {
        public ReferenceDto? History { get; set; }
        public ReferenceDto? Culture { get; set; }
        public ReferenceDto? Religion { get; set; }
    }

    public class MythCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        [StringLength(2000)] public string CreationStory { get; set; } = string.Empty;
        [StringLength(1000)] public string Symbolism { get; set; } = string.Empty;
        public bool HasReligiousConnections { get; set; }
        [Required]
        public int CultureId { get; set; }
        public int? ReligionId { get; set; }
    }

    public class MythUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        [StringLength(2000)] public string CreationStory { get; set; } = string.Empty;
        [StringLength(1000)] public string Symbolism { get; set; } = string.Empty;
        public bool HasReligiousConnections { get; set; }
        [Required]
        public int CultureId { get; set; }
        public int? ReligionId { get; set; }
    }

    // ---------------- CulturalFestival ----------------
    public class CulturalFestivalDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public string? StartDate { get; set; }
        public int DurationDays { get; set; }
        public string Activities { get; set; } = string.Empty;
        public bool IsNationalHoliday { get; set; }
        public int CultureId { get; set; }
        public int? LocationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CulturalFestivalDetailsDto : CulturalFestivalDto
    {
        public ReferenceDto? History { get; set; }
        public ReferenceDto? Culture { get; set; }
        public ReferenceDto? Location { get; set; }
    }

    public class CulturalFestivalCreateDto
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
        [StringLength(500)] public string Activities { get; set; } = string.Empty;
        public bool IsNationalHoliday { get; set; }
        [Required]
        public int CultureId { get; set; }
        public int? LocationId { get; set; }
    }

    public class CulturalFestivalUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        [StringLength(100)] public string? StartDate { get; set; }
        [Range(0, int.MaxValue)] public int DurationDays { get; set; }
        [StringLength(500)] public string Activities { get; set; } = string.Empty;
        public bool IsNationalHoliday { get; set; }
        [Required]
        public int CultureId { get; set; }
        public int? LocationId { get; set; }
    }

    // ---------------- CulturalInstitution ----------------
    public class CulturalInstitutionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        public string InstitutionType { get; set; } = string.Empty;
        public bool IsGovernmentFunded { get; set; }
        public int? CultureId { get; set; }
        public int? CityId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CulturalInstitutionDetailsDto : CulturalInstitutionDto
    {
        public ReferenceDto? History { get; set; }
        public ReferenceDto? Culture { get; set; }
        public ReferenceDto? City { get; set; }
    }

    public class CulturalInstitutionCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int WorldId { get; set; }
        public int? HistoryId { get; set; }
        [StringLength(100)] public string InstitutionType { get; set; } = string.Empty;
        public bool IsGovernmentFunded { get; set; }
        public int? CultureId { get; set; }
        public int? CityId { get; set; }
    }

    public class CulturalInstitutionUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(4000)]
        public string Description { get; set; } = string.Empty;
        public int? HistoryId { get; set; }
        [StringLength(100)] public string InstitutionType { get; set; } = string.Empty;
        public bool IsGovernmentFunded { get; set; }
        public int? CultureId { get; set; }
        public int? CityId { get; set; }
    }
}
