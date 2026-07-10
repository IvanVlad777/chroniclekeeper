using ChronicleKeeper.Core.Entities.Social.Nationality;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    /// <summary>Join: Culture ↔ Nation (composite PK, not a LoreEntity).</summary>
    public class CultureNation
    {
        public int CultureId { get; set; }
        public virtual Culture? Culture { get; set; }

        public int NationId { get; set; }
        public virtual Nation? Nation { get; set; }
    }
}
