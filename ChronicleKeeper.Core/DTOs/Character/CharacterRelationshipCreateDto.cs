using System.ComponentModel.DataAnnotations;
using static ChronicleKeeper.Core.Enums.LoreEnums;

namespace ChronicleKeeper.Core.DTOs.Character
{
    public class CharacterRelationshipCreateDto
    {
        [Required]
        public int RelatedCharacterId { get; set; }

        [Required]
        public RelationshipType Type { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        public bool IsSecret { get; set; }
    }
}
