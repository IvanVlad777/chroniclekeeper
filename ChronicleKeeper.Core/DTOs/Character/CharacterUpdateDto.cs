using System;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.Character
{
    public class CharacterUpdateDto
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
        public DateTime? DeathDate { get; set; }
        
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

        public int? SapientSpeciesId { get; set; }
        public int? RaceId { get; set; }
        public int? FatherId { get; set; }
        public int? MotherId { get; set; }

        // TODO: Otkomentirati kada budem dodavao veze
        //public int? NationId { get; set; }
        //public int? ReligionId { get; set; }
        //public int? ProfessionId { get; set; }
        //public int? SocialClassId { get; set; }
    }
}
