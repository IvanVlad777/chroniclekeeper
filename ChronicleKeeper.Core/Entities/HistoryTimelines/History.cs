using ChronicleKeeper.Core.Entities.Base;

namespace ChronicleKeeper.Core.Entities.HistoryTimelines
{
    public class History : LoreEntity
    {
        public string Summary { get; set; } = string.Empty; // Overview of historical significance
        public bool IsOfficial { get; set; } // True = Recognized history, False = Alternative or speculative

        public virtual ICollection<Timeline> Timelines { get; set; } = new List<Timeline>();
    }
}
