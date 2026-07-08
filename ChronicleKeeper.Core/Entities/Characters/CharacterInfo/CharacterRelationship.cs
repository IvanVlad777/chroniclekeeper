using System.ComponentModel.DataAnnotations;
using static ChronicleKeeper.Core.Enums.LoreEnums;

namespace ChronicleKeeper.Core.Entities.Characters.CharacterInfo
{
    /// <summary>
    /// Directed link between two characters (join entity, not a LoreEntity).
    /// World membership is implied through the owning Character.
    /// </summary>
    public class CharacterRelationship
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CharacterId { get; set; }
        public virtual Character? Character { get; set; }

        [Required]
        public int RelatedCharacterId { get; set; }
        public virtual Character? RelatedCharacter { get; set; }

        public RelationshipType Type { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsSecret { get; set; } // True if the relationship is not public knowledge
    }
}
