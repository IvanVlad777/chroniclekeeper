using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Social.Military
{
    public class MilitaryDoctrine : ILoreEntity
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

        public string Strategy { get; set; } = string.Empty; // Blitzkrieg, Guerrilla Warfare
        public string Philosophy { get; set; } = string.Empty; // "Rapid Attack", "Defense-Based"

        // ✅ Key Military Focus
        public bool PrioritizesInfantry { get; set; } // Emphasizes foot soldiers?
        public bool PrioritizesCavalry { get; set; } // Horse/mounted units?
        public bool PrioritizesArtillery { get; set; } // Heavy weapons?
        public bool PrioritizesNavalForces { get; set; } // Ships and sea power?
        public bool PrioritizesAirForces { get; set; } // Aircrafts and bombing runs?

        // ✅ Influence on Economy
        public bool RequiresHeavyIndustry { get; set; } // Large-scale production of weapons
        public bool UsesMercenaries { get; set; } // Relies on hired soldiers

        // ✅ Who uses this doctrine?
        public ICollection<MilitaryOrganization> MilitaryOrganizationsUsing { get; set; } = new List<MilitaryOrganization>();
    }

}
