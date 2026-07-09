using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.DTOs;

namespace ChronicleKeeper.Core.DTOs.Religion
{
    public class ReligionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public string CoreBeliefs { get; set; } = string.Empty;
        public string Practices { get; set; } = string.Empty;
        public bool HasDeities { get; set; }
        public bool IsStateReligion { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ReligionDetailsDto : ReligionDto
    {
        public List<ReferenceDto> Followers { get; set; } = new();
    }

    public class ReligionCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        [StringLength(4000, ErrorMessage = "Core beliefs cannot exceed 4000 characters")]
        public string CoreBeliefs { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Practices cannot exceed 4000 characters")]
        public string Practices { get; set; } = string.Empty;

        public bool HasDeities { get; set; }
        public bool IsStateReligion { get; set; }
    }

    public class ReligionUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Core beliefs cannot exceed 4000 characters")]
        public string CoreBeliefs { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Practices cannot exceed 4000 characters")]
        public string Practices { get; set; } = string.Empty;

        public bool HasDeities { get; set; }
        public bool IsStateReligion { get; set; }
    }
}
