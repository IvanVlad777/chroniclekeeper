using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class Industry : LoreEntity
    {
        public string Sector { get; set; } = string.Empty; // Agriculture, Mining, Manufacturing
        public double EmploymentRate { get; set; } // % of population employed

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }
    }
}
