using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class ExtractionMethod : ILoreEntity
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

        public string MethodType { get; set; } = string.Empty; // Mining, Farming, Drilling, Alchemy
        public bool IsSustainable { get; set; } // True if eco-friendly

        public ICollection<NaturalResource> ResourcesExtracted { get; set; } = new List<NaturalResource>(); // ✅ Resources gathered
    }
}
