using System.ComponentModel.DataAnnotations;

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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class SpeciesDetailsDto : SpeciesDto
    {
        public List<RaceDto> Races { get; set; } = new();
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
