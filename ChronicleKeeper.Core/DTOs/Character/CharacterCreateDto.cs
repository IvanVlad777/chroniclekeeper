using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.Character
{
    // Full field set (mirrors CharacterUpdateDto + WorldId) — create accepts everything update does,
    // so the client no longer needs a POST-then-PUT dance to fill the extended fields.
    public class CharacterCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Nickname cannot exceed 50 characters")]
        public string Nickname { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Birth date cannot exceed 100 characters")]
        public string? BirthDate { get; set; }

        [StringLength(100, ErrorMessage = "Death date cannot exceed 100 characters")]
        public string? DeathDate { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = string.Empty;

        [Range(0, 300, ErrorMessage = "Height must be between 0 and 300 cm")]
        public double? Height { get; set; }

        [Range(0, 1000, ErrorMessage = "Weight must be between 0 and 1000 kg")]
        public double? Weight { get; set; }

        [StringLength(50, ErrorMessage = "Hair color cannot exceed 50 characters")]
        public string HairColor { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Eye color cannot exceed 50 characters")]
        public string EyeColor { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Special physical features cannot exceed 500 characters")]
        public string SpecialPhysicalFeatures { get; set; } = string.Empty;

        public bool IsArtificial { get; set; }

        [Required]
        public int WorldId { get; set; }

        public int? SapientSpeciesId { get; set; }
        public int? RaceId { get; set; }
        public int? SocialClassId { get; set; }
        public int? NationId { get; set; }
        public int? ReligionId { get; set; }
        public int? FatherId { get; set; }
        public int? MotherId { get; set; }

        public int? ProfessionId { get; set; }
        public int? HistoryId { get; set; }

        public BackgroundInfoDto Background { get; set; } = new();
        public PersonalityInfoDto Personality { get; set; } = new();
    }
}
