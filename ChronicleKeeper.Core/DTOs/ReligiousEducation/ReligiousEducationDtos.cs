using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.ReligiousEducation
{
    public class ReligiousEducationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? CharacterId { get; set; }
        public int ReligionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public bool Ordained { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ReligiousEducationCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>Svijet vjerskog obrazovanja se izvodi iz religije — ne šalje se WorldId.</summary>
        [Required]
        public int ReligionId { get; set; }

        public int? CharacterId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public bool Ordained { get; set; }
    }

    public class ReligiousEducationUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public int? CharacterId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? CompletionDate { get; set; }

        public bool Ordained { get; set; }
    }
}
