using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.CorporateLeadership
{
    public class CorporateLeadershipDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int CorporationId { get; set; }
        public string Position { get; set; } = string.Empty;
        public double Salary { get; set; }
        public bool IsMajorShareholder { get; set; }
        public int? ProfessionId { get; set; }
        public string? ProfessionName { get; set; }
        public int? CharacterId { get; set; }
        public string? CharacterName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CorporateLeadershipCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>Svijet pozicije se izvodi iz korporacije — ne šalje se WorldId.</summary>
        [Required]
        public int CorporationId { get; set; }

        [StringLength(100)]
        public string Position { get; set; } = string.Empty;

        public double Salary { get; set; }
        public bool IsMajorShareholder { get; set; }

        public int? ProfessionId { get; set; }
        public int? CharacterId { get; set; }
    }

    public class CorporateLeadershipUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(100)]
        public string Position { get; set; } = string.Empty;

        public double Salary { get; set; }
        public bool IsMajorShareholder { get; set; }

        public int? ProfessionId { get; set; }
        public int? CharacterId { get; set; }
    }
}
