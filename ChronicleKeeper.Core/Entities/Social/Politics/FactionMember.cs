using ChronicleKeeper.Core.Entities.Characters;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Social
{
    /// <summary>
    /// Membership of a character in a faction (join entity, not a LoreEntity).
    /// </summary>
    public class FactionMember
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FactionId { get; set; }
        public virtual Faction? Faction { get; set; }

        [Required]
        public int CharacterId { get; set; }
        public virtual Character? Character { get; set; }

        public string Role { get; set; } = string.Empty; // e.g. "Guildmaster", "Spy"
        public bool IsSecret { get; set; } // True if the membership is not public knowledge
    }
}
