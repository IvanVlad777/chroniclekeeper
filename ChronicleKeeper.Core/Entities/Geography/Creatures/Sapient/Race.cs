using ChronicleKeeper.Core.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient
{
    /// <summary>
    /// Each Race belongs to one SapientSpecies; a SapientSpecies can have multiple Races.
    /// </summary>
    public class Race : LoreEntity
    {
        [Required]
        public int SapientSpeciesId { get; set; }
        public virtual SapientSpecies? SapientSpecies { get; set; }

        public string AppearanceTraits { get; set; } = string.Empty; // Physical traits
        public string GeneticFeatures { get; set; } = string.Empty; // DNA/biological uniqueness
        public string Adaptations { get; set; } = string.Empty; // Evolutionary traits (e.g., Night Vision)
    }
}
