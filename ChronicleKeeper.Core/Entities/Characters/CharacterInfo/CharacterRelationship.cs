using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Characters.CharacterInfo
{
    public class CharacterRelationship : ILoreEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public virtual History? History { get; set; }

        //[Required]
        //[ForeignKey("SourceCharacter")]
        //public int SourceCharacterId { get; private set; }
        //public Character? SourceCharacter { get; set; }
        //[Required]
        //[ForeignKey("TargetCharacter")]
        //public int TargetCharacterId { get; private set; }
        //public Character? TargetCharacter { get; set; }

        //public string RelationshipType { get; set; } = string.Empty; // Friend, Enemy, Rival, Romantic Partner
        //public bool IsSecret { get; set; } // True if the relationship is not public knowledge

        //public void SetRelationship(Character source, Character target)
        //{
        //    if (source.Id == target.Id)
        //    {
        //        throw new InvalidOperationException("A character cannot have a relationship with themselves.");
        //    }
        //    SourceCharacterId = source.Id;
        //    TargetCharacterId = target.Id;
        //}
    }
}
