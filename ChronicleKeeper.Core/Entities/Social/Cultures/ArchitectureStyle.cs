using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class ArchitectureStyle : ILoreEntity
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

        public string MaterialsUsed { get; set; } = string.Empty; // Stone, wood, clay, metal
        public string DesignFeatures { get; set; } = string.Empty; // Domes, arches, towers
        public bool IsFortified { get; set; } // If commonly used for defense

        public ICollection<Location> TypicalLocations { get; set; } = new List<Location>();

        //[ForeignKey("Culture")]
        public int CultureId { get; set; }
        public Culture Culture { get; set; } = null!;
    }

}
