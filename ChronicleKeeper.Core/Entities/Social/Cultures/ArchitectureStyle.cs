using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class ArchitectureStyle : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public string MaterialsUsed { get; set; } = string.Empty; // Stone, wood, clay, metal
        public string DesignFeatures { get; set; } = string.Empty; // Domes, arches, towers
        public bool IsFortified { get; set; } // If commonly used for defense

        public int CultureId { get; set; }
        public virtual Culture? Culture { get; set; }

        // M:N with Location (join entity) — owner side.
        public virtual ICollection<ArchitectureStyleLocation> TypicalLocations { get; set; } = new List<ArchitectureStyleLocation>();
    }
}
