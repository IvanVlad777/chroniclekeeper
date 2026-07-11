using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.DTOs;

namespace ChronicleKeeper.Core.DTOs.Library
{
    public class LibraryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public bool IsPublic { get; set; }
        public bool FocusesOnMagic { get; set; }
        public bool FocusesOnHistory { get; set; }
        public int? UniversityId { get; set; }
        public ReferenceDto? University { get; set; }
        public int? LocationId { get; set; }
        public ReferenceDto? Location { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class LibraryDetailsDto : LibraryDto
    {
    }

    public class LibraryCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        public bool IsPublic { get; set; }
        public bool FocusesOnMagic { get; set; }
        public bool FocusesOnHistory { get; set; }

        public int? UniversityId { get; set; }
        public int? LocationId { get; set; }
    }

    public class LibraryUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public bool IsPublic { get; set; }
        public bool FocusesOnMagic { get; set; }
        public bool FocusesOnHistory { get; set; }

        public int? UniversityId { get; set; }
        public int? LocationId { get; set; }
    }
}
