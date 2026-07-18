using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Religions;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class Tradition : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public string Practice { get; set; } = string.Empty; // Wedding ceremonies, rites of passage
        public bool IsReligious { get; set; } // If tied to a religion

        // Optional religion link — SetNull.
        public int? ReligionId { get; set; }
        public virtual Religion? Religion { get; set; }

        public int CultureId { get; set; }
        public virtual Culture? Culture { get; set; }
    }
}
