using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.Specialisation
{
    public class SpecialisationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int? ProfessionId { get; set; }
        public string Field { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class SpecialisationCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>Svijet specijalizacije se izvodi iz zanimanja — ne šalje se WorldId.</summary>
        [Required]
        public int ProfessionId { get; set; }

        [StringLength(200)]
        public string Field { get; set; } = string.Empty;
    }

    public class SpecialisationUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(200)]
        public string Field { get; set; } = string.Empty;
    }
}
