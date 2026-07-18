using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Nationality;

namespace ChronicleKeeper.Core.Entities.Social.Structure
{
    public class SocialHierarchy : LoreEntity
    {
        public bool IsCasteSystem { get; set; }          // True = Fixed classes, False = Social mobility
        public bool AllowsUpwardMobility { get; set; }   // Can lower classes rise?
        public bool AllowsIntermarriage { get; set; }    // Can nobles marry commoners?
        public bool EnforcesLegalSeparation { get; set; } // True if laws divide classes

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        // Reverse navs — the FK lives on the child side (SocialClass.SocialHierarchyId,
        // Nation.SocialHierarchyId), both optional/SetNull.
        public ICollection<SocialClass> Classes { get; set; } = new List<SocialClass>();
        public ICollection<Nation> Nations { get; set; } = new List<Nation>();
    }
}
