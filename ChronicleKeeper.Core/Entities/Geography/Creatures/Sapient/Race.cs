using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography.Ecosystems;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient
{
    public class Race : ILoreEntity
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

        //[ForeignKey("SapientSpecies")]
        public int SapientSpeciesId { get; set; } // ✅ Every Race belongs to ONE SapientSpecies
        public SapientSpecies? SapientSpecies { get; set; }

        public string AppearanceTraits { get; set; } = string.Empty; // Physical traits
        public string GeneticFeatures { get; set; } = string.Empty; // DNA/biological uniqueness
        public string Adaptations { get; set; } = string.Empty; // Evolutionary traits (e.g., Night Vision)
    }

    //Key Features:
    //Each Race belongs to one SapientSpecies.
    //A SapientSpecies can have multiple Races.
}
