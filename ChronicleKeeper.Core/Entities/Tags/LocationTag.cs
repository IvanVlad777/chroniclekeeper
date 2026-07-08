using ChronicleKeeper.Core.Entities.Geography;

namespace ChronicleKeeper.Core.Entities.Tags
{
    /// <summary>Join: Location ↔ Tag (composite PK, not a LoreEntity).</summary>
    public class LocationTag
    {
        public int LocationId { get; set; }
        public virtual Location? Location { get; set; }

        public int TagId { get; set; }
        public virtual Tag? Tag { get; set; }
    }
}
