using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.Enums;
using static ChronicleKeeper.Core.Enums.CreatureEnums;
using static ChronicleKeeper.Core.Enums.GlobalEnums;

namespace ChronicleKeeper.Core.DTOs.Creature
{
    /// <summary>
    /// Flat DTO for the Animal/Plant/Tree/Crop/Fungus TPH family. Only the fields relevant to
    /// <see cref="Subtype"/> are populated — every other subtype-specific field stays null.
    /// NOTE: <see cref="Type"/> is the CreatureType classification enum (Sapient/Animal/Plant/
    /// Fungus/Spiritual/Mythical) — a loose in-world label, NOT the TPH discriminator. It does not
    /// map 1:1 onto <see cref="Subtype"/> (e.g. no C# class exists for Sapient/Spiritual/Mythical),
    /// so the two fields are independent; don't conflate them.
    /// </summary>
    public class CreatureDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }

        /// <summary>"Animal" | "Plant" | "Tree" | "Crop" | "Fungus" — mirrors the "CreatureSubtype" TPH discriminator.</summary>
        public string Subtype { get; set; } = string.Empty;

        public CreatureType Type { get; set; }
        public double AverageLifespan { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public bool IsSentient { get; set; }
        public bool IsArtificial { get; set; }
        public ArtificialOrigin? ArtificialOrigin { get; set; }
        public int? ParentCreatureId { get; set; }
        public int? HistoryId { get; set; }

        // Plant / Fungus (shared field names)
        public string? ScientificName { get; set; }
        public bool? IsMedicinal { get; set; }
        public bool? IsPoisonous { get; set; }
        public bool? IsBioluminescent { get; set; }
        public bool? IsSymbiotic { get; set; }
        public string? SpecialProperties { get; set; }
        public string? MythologicalSignificance { get; set; }

        // Animal / Crop (shared field name)
        public bool? IsDomesticated { get; set; }

        // Animal
        public DietType? Diet { get; set; }
        public int? NumberOfLegs { get; set; }
        public bool? HasWings { get; set; }
        public bool? HasMultipleHeads { get; set; }
        public bool? HasRegeneration { get; set; }
        public bool? IsSacred { get; set; }
        public bool? IsMythical { get; set; }
        public bool? IsEndangered { get; set; }
        public string? Intelligence { get; set; }
        public string? SpecialAbilities { get; set; }
        public bool? IsPackAnimal { get; set; }
        public bool? IsAggressive { get; set; }

        // Plant
        public PlantType? PlantType { get; set; }
        public SunlightRequirement? Sunlight { get; set; }
        public SoilType? PreferredSoil { get; set; }
        public TemperatureRange? TemperatureRange { get; set; }
        public Rarity? Rarity { get; set; }
        public bool? IsCarnivorous { get; set; }
        public bool? HasRegenerativeProperties { get; set; }
        public bool? CanMove { get; set; }
        public bool? IsParasitic { get; set; }

        // Tree
        public double? MaxHeight { get; set; }
        public int? Lifespan { get; set; }
        public LeafType? LeafType { get; set; }

        // Crop
        public double? YieldPerHectare { get; set; }
        public CropType? CropType { get; set; }

        // Fungus
        public bool? IsEdible { get; set; }
        public bool? IsHallucinogenic { get; set; }
        public bool? HasMutagenicProperties { get; set; }
        public bool? CanCommunicate { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreatureDetailsDto : CreatureDto
    {
        public ReferenceDto? ParentCreature { get; set; }
        public List<ReferenceDto> Subspecies { get; set; } = new();
        public ReferenceDto? History { get; set; }
        public List<ReferenceDto> Cities { get; set; } = new();
        public List<ReferenceDto> Habitats { get; set; } = new();
        public List<ReferenceDto> SymbioticPartners { get; set; } = new();
        public List<ReferenceDto> Prey { get; set; } = new();
        public List<ReferenceDto> Predators { get; set; } = new();  // read-only reverse
    }

    public class CreatureCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        /// <summary>"Animal" | "Plant" | "Tree" | "Crop" | "Fungus" — selects the concrete TPH subtype.</summary>
        [Required]
        public string Subtype { get; set; } = string.Empty;

        public CreatureType Type { get; set; }
        public double AverageLifespan { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public bool IsSentient { get; set; }
        public bool IsArtificial { get; set; }
        public ArtificialOrigin? ArtificialOrigin { get; set; }
        public int? ParentCreatureId { get; set; }
        public int? HistoryId { get; set; }

        [StringLength(100)]
        public string? ScientificName { get; set; }
        public bool? IsMedicinal { get; set; }
        public bool? IsPoisonous { get; set; }
        public bool? IsBioluminescent { get; set; }
        public bool? IsSymbiotic { get; set; }
        [StringLength(500)]
        public string? SpecialProperties { get; set; }
        [StringLength(500)]
        public string? MythologicalSignificance { get; set; }

        public bool? IsDomesticated { get; set; }

        public DietType? Diet { get; set; }
        public int? NumberOfLegs { get; set; }
        public bool? HasWings { get; set; }
        public bool? HasMultipleHeads { get; set; }
        public bool? HasRegeneration { get; set; }
        public bool? IsSacred { get; set; }
        public bool? IsMythical { get; set; }
        public bool? IsEndangered { get; set; }
        [StringLength(500)]
        public string? Intelligence { get; set; }
        [StringLength(500)]
        public string? SpecialAbilities { get; set; }
        public bool? IsPackAnimal { get; set; }
        public bool? IsAggressive { get; set; }

        public PlantType? PlantType { get; set; }
        public SunlightRequirement? Sunlight { get; set; }
        public SoilType? PreferredSoil { get; set; }
        public TemperatureRange? TemperatureRange { get; set; }
        public Rarity? Rarity { get; set; }
        public bool? IsCarnivorous { get; set; }
        public bool? HasRegenerativeProperties { get; set; }
        public bool? CanMove { get; set; }
        public bool? IsParasitic { get; set; }

        public double? MaxHeight { get; set; }
        public int? Lifespan { get; set; }
        public LeafType? LeafType { get; set; }

        public double? YieldPerHectare { get; set; }
        public CropType? CropType { get; set; }

        public bool? IsEdible { get; set; }
        public bool? IsHallucinogenic { get; set; }
        public bool? HasMutagenicProperties { get; set; }
        public bool? CanCommunicate { get; set; }
    }

    /// <summary>Same as <see cref="CreatureCreateDto"/> minus WorldId and Subtype — a Creature's
    /// concrete type can't change after creation (same convention as ContentUpdateDto).</summary>
    public class CreatureUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public CreatureType Type { get; set; }
        public double AverageLifespan { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public bool IsSentient { get; set; }
        public bool IsArtificial { get; set; }
        public ArtificialOrigin? ArtificialOrigin { get; set; }
        public int? ParentCreatureId { get; set; }
        public int? HistoryId { get; set; }

        [StringLength(100)]
        public string? ScientificName { get; set; }
        public bool? IsMedicinal { get; set; }
        public bool? IsPoisonous { get; set; }
        public bool? IsBioluminescent { get; set; }
        public bool? IsSymbiotic { get; set; }
        [StringLength(500)]
        public string? SpecialProperties { get; set; }
        [StringLength(500)]
        public string? MythologicalSignificance { get; set; }

        public bool? IsDomesticated { get; set; }

        public DietType? Diet { get; set; }
        public int? NumberOfLegs { get; set; }
        public bool? HasWings { get; set; }
        public bool? HasMultipleHeads { get; set; }
        public bool? HasRegeneration { get; set; }
        public bool? IsSacred { get; set; }
        public bool? IsMythical { get; set; }
        public bool? IsEndangered { get; set; }
        [StringLength(500)]
        public string? Intelligence { get; set; }
        [StringLength(500)]
        public string? SpecialAbilities { get; set; }
        public bool? IsPackAnimal { get; set; }
        public bool? IsAggressive { get; set; }

        public PlantType? PlantType { get; set; }
        public SunlightRequirement? Sunlight { get; set; }
        public SoilType? PreferredSoil { get; set; }
        public TemperatureRange? TemperatureRange { get; set; }
        public Rarity? Rarity { get; set; }
        public bool? IsCarnivorous { get; set; }
        public bool? HasRegenerativeProperties { get; set; }
        public bool? CanMove { get; set; }
        public bool? IsParasitic { get; set; }

        public double? MaxHeight { get; set; }
        public int? Lifespan { get; set; }
        public LeafType? LeafType { get; set; }

        public double? YieldPerHectare { get; set; }
        public CropType? CropType { get; set; }

        public bool? IsEdible { get; set; }
        public bool? IsHallucinogenic { get; set; }
        public bool? HasMutagenicProperties { get; set; }
        public bool? CanCommunicate { get; set; }
    }
}
