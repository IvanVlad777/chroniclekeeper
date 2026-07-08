using System.ComponentModel.DataAnnotations;
using static ChronicleKeeper.Core.Enums.SocietyEnums;

namespace ChronicleKeeper.Core.DTOs.Faction
{
    public class FactionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public FactionType Type { get; set; }
        public bool IsSecretive { get; set; }
        public string Motto { get; set; } = string.Empty;
        public int? LeaderId { get; set; }
        public int? HeadquartersId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class FactionDetailsDto : FactionDto
    {
        public ReferenceDto? Leader { get; set; }
        public ReferenceDto? Headquarters { get; set; }
        public List<FactionMemberDto> Members { get; set; } = new();
        public List<ReferenceDto> Tags { get; set; } = new();
    }

    public class FactionMemberDto
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public string CharacterName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsSecret { get; set; }
    }

    public class FactionCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }

        public FactionType Type { get; set; }
        public bool IsSecretive { get; set; }

        [StringLength(200)]
        public string Motto { get; set; } = string.Empty;

        public int? LeaderId { get; set; }
        public int? HeadquartersId { get; set; }
    }

    public class FactionUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        public FactionType Type { get; set; }
        public bool IsSecretive { get; set; }

        [StringLength(200)]
        public string Motto { get; set; } = string.Empty;

        public int? LeaderId { get; set; }
        public int? HeadquartersId { get; set; }
    }

    public class FactionMemberAddDto
    {
        [Required]
        public int CharacterId { get; set; }

        [StringLength(100)]
        public string Role { get; set; } = string.Empty;

        public bool IsSecret { get; set; }
    }
}
