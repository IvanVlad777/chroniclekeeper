using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class ArtForm : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public string Type { get; set; } = string.Empty; // Music, Painting, Sculpture, Dance
        public string NotableArtists { get; set; } = string.Empty; // Famous figures in this art form
        public string HistoricalInfluences { get; set; } = string.Empty;

        public int CultureId { get; set; }
        public virtual Culture? Culture { get; set; }
    }
}
