using System.ComponentModel.DataAnnotations;
using static ChronicleKeeper.Core.Enums.CreatureEnums;

namespace ChronicleKeeper.Core.DTOs.Species
{
    public class SpeciesDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public string CommonName { get; set; } = string.Empty;
        public string ScientificName { get; set; } = string.Empty;
        public bool IsHumanoid { get; set; }
        public string Lifespan { get; set; } = string.Empty;

        // Inherited from Creature (SapientSpecies is now a Creature TPH subtype)
        public SapientType SapientType { get; set; }
        public CreatureType Type { get; set; } // always Sapient — read-only classification
        public double AverageLifespan { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public bool IsSentient { get; set; }
        public bool IsArtificial { get; set; }
        public ArtificialOrigin? ArtificialOrigin { get; set; }
        public int? ParentCreatureId { get; set; }
        public int? HistoryId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class SpeciesDetailsDto : SpeciesDto
    {
        public List<RaceDto> Races { get; set; } = new();
        public ReferenceDto? ParentCreature { get; set; }
        public List<ReferenceDto> Subspecies { get; set; } = new();
        public ReferenceDto? History { get; set; }
    }

    public class SpeciesCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [StringLength(100)]
        public string CommonName { get; set; } = string.Empty;

        [StringLength(100)]
        public string ScientificName { get; set; } = string.Empty;

        public bool IsHumanoid { get; set; }

        [StringLength(100)]
        public string Lifespan { get; set; } = string.Empty;

        public SapientType SapientType { get; set; }
        public double AverageLifespan { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public bool IsSentient { get; set; } = true;
        public bool IsArtificial { get; set; }
        public ArtificialOrigin? ArtificialOrigin { get; set; }
        public int? ParentCreatureId { get; set; }
        public int? HistoryId { get; set; }
    }

    public class SpeciesUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(100)]
        public string CommonName { get; set; } = string.Empty;

        [StringLength(100)]
        public string ScientificName { get; set; } = string.Empty;

        public bool IsHumanoid { get; set; }

        [StringLength(100)]
        public string Lifespan { get; set; } = string.Empty;

        public SapientType SapientType { get; set; }
        public double AverageLifespan { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public bool IsSentient { get; set; } = true;
        public bool IsArtificial { get; set; }
        public ArtificialOrigin? ArtificialOrigin { get; set; }
        public int? ParentCreatureId { get; set; }
        public int? HistoryId { get; set; }
    }

    public class RaceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int SapientSpeciesId { get; set; }
        public string AppearanceTraits { get; set; } = string.Empty;
        public string GeneticFeatures { get; set; } = string.Empty;
        public string Adaptations { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class RaceCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>Svijet rase se izvodi iz vrste — ne šalje se WorldId.</summary>
        [Required]
        public int SapientSpeciesId { get; set; }

        [StringLength(500)]
        public string AppearanceTraits { get; set; } = string.Empty;

        [StringLength(500)]
        public string GeneticFeatures { get; set; } = string.Empty;

        [StringLength(500)]
        public string Adaptations { get; set; } = string.Empty;
    }

    // Vrsta rase se ne mijenja (mijenjala bi invarijantu likova) — zato bez SapientSpeciesId
    public class RaceUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(500)]
        public string AppearanceTraits { get; set; } = string.Empty;

        [StringLength(500)]
        public string GeneticFeatures { get; set; } = string.Empty;

        [StringLength(500)]
        public string Adaptations { get; set; } = string.Empty;
    }
}
