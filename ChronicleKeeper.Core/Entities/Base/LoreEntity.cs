using ChronicleKeeper.Core.Entities.Worlds;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Base
{
    /// <summary>
    /// Base class for every lore entity. All worldbuilding data belongs to exactly one World.
    /// </summary>
    public abstract class LoreEntity : ILoreEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [Required]
        public int WorldId { get; set; }
        public virtual World? World { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        //public virtual History? History { get; set; } // TODO: Uncomment when History entity is revived
    }
}
