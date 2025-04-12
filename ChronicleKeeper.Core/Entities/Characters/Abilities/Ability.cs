using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Professions;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;

namespace ChronicleKeeper.Core.Entities.Characters.Abilities
{
    public class Ability : ILoreEntity
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
        public string Type { get; set; } = string.Empty; // Physical, Magical, Mental, Technical

        //public int? AbilityLevelId { get; set; }
        public AbilityLevel? AbilityLevel { get; set; }

        public ICollection<Character> Characters { get; set; } = new List<Character>();
    }

}
