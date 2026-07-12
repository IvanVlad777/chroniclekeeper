using System.ComponentModel.DataAnnotations;
using static ChronicleKeeper.Core.Enums.EcosystemEnums;
using static ChronicleKeeper.Core.Enums.LoreEnums;

namespace ChronicleKeeper.Core.DTOs.Location
{
    public class LocationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public LocationType Type { get; set; }
        public double? Area { get; set; }
        public int? Population { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? ParentLocationId { get; set; }
        public int? HistoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Subtype fields — only populated for the matching Type; null/empty otherwise.
        public string? ContinentSpecifics { get; set; }
        public string? RegionSpecifics { get; set; }
        public int? GovernmentSystemId { get; set; }
        public int? LegalSystemId { get; set; }
        public int? EducationSystemId { get; set; }
        public bool? IsCapital { get; set; }
        public string? DistrictType { get; set; }

        // Ecosystem hierarchy (shared base field)
        public string? UniqueFeatures { get; set; }
        // WaterEcosystem (Lake/Sea/Ocean/River)
        public double? WaterDepth { get; set; }
        // LakeEcosystem
        public double? Volume { get; set; }
        public double? MaxDepth { get; set; }
        public bool? IsFreshwater { get; set; }
        // RiverEcosystem
        public double? RiverLength { get; set; }
        public int? SourceLocationId { get; set; }
        public int? MouthLocationId { get; set; }
        // MountainEcosystem
        public double? MaxElevation { get; set; }
        public double? Prominence { get; set; }
        // MountainRange
        public double? MountainRangeLength { get; set; }
        // SwampEcosystem
        public bool? IsSaltwater { get; set; }
        // DesertEcosystem / ForestEcosystem / CaveEcosystem / GrasslandEcosystem
        public DesertType? DesertKind { get; set; }
        public ForestType? ForestKind { get; set; }
        public double? CaveDepth { get; set; }
        public CaveType? CaveKind { get; set; }
        public GrasslandType? GrasslandKind { get; set; }
    }

    public class LocationDetailsDto : LocationDto
    {
        public ReferenceDto? ParentLocation { get; set; }
        public List<ReferenceDto> SubLocations { get; set; } = new();
        public List<ReferenceDto> Tags { get; set; } = new();
        public ReferenceDto? History { get; set; }
        public ReferenceDto? GovernmentSystem { get; set; }
        public ReferenceDto? LegalSystem { get; set; }
        public ReferenceDto? EducationSystem { get; set; }
        public List<ReferenceDto> Schools { get; set; } = new();
        public List<ReferenceDto> NativeSpecies { get; set; } = new();
        public ReferenceDto? SourceLocation { get; set; }
        public ReferenceDto? MouthLocation { get; set; }
    }

    public class LocationCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        public LocationType Type { get; set; } = LocationType.Other;
        public double? Area { get; set; }
        public int? Population { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? ParentLocationId { get; set; }
        public int? HistoryId { get; set; }

        public string? ContinentSpecifics { get; set; }
        public string? RegionSpecifics { get; set; }
        public int? GovernmentSystemId { get; set; }
        public int? LegalSystemId { get; set; }
        public int? EducationSystemId { get; set; }
        public bool? IsCapital { get; set; }
        public string? DistrictType { get; set; }

        public string? UniqueFeatures { get; set; }
        public double? WaterDepth { get; set; }
        public double? Volume { get; set; }
        public double? MaxDepth { get; set; }
        public bool? IsFreshwater { get; set; }
        public double? RiverLength { get; set; }
        public int? SourceLocationId { get; set; }
        public int? MouthLocationId { get; set; }
        public double? MaxElevation { get; set; }
        public double? Prominence { get; set; }
        public double? MountainRangeLength { get; set; }
        public bool? IsSaltwater { get; set; }
        public DesertType? DesertKind { get; set; }
        public ForestType? ForestKind { get; set; }
        public double? CaveDepth { get; set; }
        public CaveType? CaveKind { get; set; }
        public GrasslandType? GrasslandKind { get; set; }
    }

    public class LocationUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public LocationType Type { get; set; } = LocationType.Other;
        public double? Area { get; set; }
        public int? Population { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? ParentLocationId { get; set; }
        public int? HistoryId { get; set; }

        public string? ContinentSpecifics { get; set; }
        public string? RegionSpecifics { get; set; }
        public int? GovernmentSystemId { get; set; }
        public int? LegalSystemId { get; set; }
        public int? EducationSystemId { get; set; }
        public bool? IsCapital { get; set; }
        public string? DistrictType { get; set; }

        public string? UniqueFeatures { get; set; }
        public double? WaterDepth { get; set; }
        public double? Volume { get; set; }
        public double? MaxDepth { get; set; }
        public bool? IsFreshwater { get; set; }
        public double? RiverLength { get; set; }
        public int? SourceLocationId { get; set; }
        public int? MouthLocationId { get; set; }
        public double? MaxElevation { get; set; }
        public double? Prominence { get; set; }
        public double? MountainRangeLength { get; set; }
        public bool? IsSaltwater { get; set; }
        public DesertType? DesertKind { get; set; }
        public ForestType? ForestKind { get; set; }
        public double? CaveDepth { get; set; }
        public CaveType? CaveKind { get; set; }
        public GrasslandType? GrasslandKind { get; set; }
    }
}
