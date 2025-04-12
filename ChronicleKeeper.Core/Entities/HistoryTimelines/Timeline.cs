using ChronicleKeeper.Core.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ChronicleKeeper.Core.Entities.HistoryTimelines
{
    public class Timeline
    {
        public int Id { get; set; }

        [Required]
        public string PeriodName { get; set; } = string.Empty; // e.g., "Golden Age", "Dark Era"
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; } // Can be ongoing if null

        //[ForeignKey("History")]
        public int HistoryId { get; set; }// ✅ Linked to a historical record
        public History History { get; set; } = null!;

        public ICollection<TimelineEvent> Events { get; set; } = new List<TimelineEvent>(); // ✅ Historical events in order??
    }

}
