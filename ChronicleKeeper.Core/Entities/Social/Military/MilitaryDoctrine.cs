using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Military
{
    public class MilitaryDoctrine : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public string Strategy { get; set; } = string.Empty; // e.g., Blitzkrieg, Guerrilla Warfare
        public string Philosophy { get; set; } = string.Empty; // e.g., "Rapid Attack", "Defense-Based"

        // Key military focus
        public bool PrioritizesInfantry { get; set; }
        public bool PrioritizesCavalry { get; set; }
        public bool PrioritizesArtillery { get; set; }
        public bool PrioritizesNavalForces { get; set; }
        public bool PrioritizesAirForces { get; set; }

        // Influence on economy
        public bool RequiresHeavyIndustry { get; set; }
        public bool UsesMercenaries { get; set; }

        // Reverse of MilitaryOrganization.MilitaryDoctrineId
        public virtual ICollection<MilitaryOrganization> MilitaryOrganizationsUsing { get; set; } = new List<MilitaryOrganization>();
    }
}
