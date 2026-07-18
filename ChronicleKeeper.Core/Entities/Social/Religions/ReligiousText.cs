using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Religions
{
    public class ReligiousText : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public string Type { get; set; } = string.Empty; // Scripture, Mythology, Prophecy
        public string ContentSummary { get; set; } = string.Empty; // Short summary of teachings

        public int ReligionId { get; set; }
        public virtual Religion? Religion { get; set; }

        // Optional deity link — SetNull.
        public int? DeityId { get; set; }
        public virtual Deity? Deity { get; set; }
    }
}
