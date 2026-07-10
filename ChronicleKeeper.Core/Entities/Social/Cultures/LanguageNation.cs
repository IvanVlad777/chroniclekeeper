using ChronicleKeeper.Core.Entities.Social.Nationality;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    /// <summary>Join: Language ↔ Nation (composite PK, not a LoreEntity).</summary>
    public class LanguageNation
    {
        public int LanguageId { get; set; }
        public virtual Language? Language { get; set; }

        public int NationId { get; set; }
        public virtual Nation? Nation { get; set; }
    }
}
