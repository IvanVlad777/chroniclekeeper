using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.DTOs;

namespace ChronicleKeeper.Core.DTOs.EducationSystem
{
    public class EducationSystemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public bool IsStateControlled { get; set; }
        public bool AllowsPrivateInstitutions { get; set; }
        public bool IncludesReligiousEducation { get; set; }
        public bool SupportsGuildTraining { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class EducationSystemDetailsDto : EducationSystemDto
    {
        public List<ReferenceDto> Schools { get; set; } = new();
        public List<ReferenceDto> Universities { get; set; } = new();
    }

    public class EducationSystemCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        public bool IsStateControlled { get; set; }
        public bool AllowsPrivateInstitutions { get; set; }
        public bool IncludesReligiousEducation { get; set; }
        public bool SupportsGuildTraining { get; set; }
    }

    public class EducationSystemUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public bool IsStateControlled { get; set; }
        public bool AllowsPrivateInstitutions { get; set; }
        public bool IncludesReligiousEducation { get; set; }
        public bool SupportsGuildTraining { get; set; }
    }
}
