using System;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.Character
{
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
        
        public DateTime? BirthDate { get; set; }
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
    }
}
