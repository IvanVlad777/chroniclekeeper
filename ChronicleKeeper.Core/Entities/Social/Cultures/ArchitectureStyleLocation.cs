using ChronicleKeeper.Core.Entities.Geography;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    /// <summary>Join: ArchitectureStyle ↔ Location (where this style is typical). Composite PK, not a LoreEntity.</summary>
    public class ArchitectureStyleLocation
    {
        public int ArchitectureStyleId { get; set; }
        public virtual ArchitectureStyle? ArchitectureStyle { get; set; }

        public int LocationId { get; set; }
        public virtual Location? Location { get; set; }
    }
}
