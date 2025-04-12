using ChronicleKeeper.Core.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.HistoryTimelines
{
    public class History
    {
        public int Id { get; set; }
        public string Summary { get; set; } = string.Empty; // Overview of historical significance
        public bool IsOfficial { get; set; } // True = Recognized history, False = Alternative or speculative

        //[ForeignKey("LoreEntity")]
        //public int LoreEntityId { get; set; } // ✅ Any LoreEntity can have history

        public ICollection<Timeline> Timelines { get; set; } = new List<Timeline>(); // ✅ Multiple timelines supported
    }
}
