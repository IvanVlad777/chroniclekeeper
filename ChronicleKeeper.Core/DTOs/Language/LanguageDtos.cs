using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.DTOs;

namespace ChronicleKeeper.Core.DTOs.Language
{
    public class LanguageDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public string WritingSystem { get; set; } = string.Empty;
        public bool IsExtinct { get; set; }
        public string Dialects { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class LanguageDetailsDto : LanguageDto
    {
        public List<ReferenceDto> Cultures { get; set; } = new();
    }

    public class LanguageCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [StringLength(100)]
        public string WritingSystem { get; set; } = string.Empty;

        public bool IsExtinct { get; set; }

        [StringLength(500)]
        public string Dialects { get; set; } = string.Empty;
    }

    public class LanguageUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(100)]
        public string WritingSystem { get; set; } = string.Empty;

        public bool IsExtinct { get; set; }

        [StringLength(500)]
        public string Dialects { get; set; } = string.Empty;
    }
}
