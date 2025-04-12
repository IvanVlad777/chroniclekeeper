using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Entities.Social.Military;
using System.ComponentModel.DataAnnotations.Schema;


namespace ChronicleKeeper.Core.Entities.HistoryTimelines
{
    public class TimelineEvent
    {
        public int Id { get; set; }
        public DateTime EventDate { get; set; }
        public string Consequences { get; set; } = string.Empty; // Impact on history
        public bool IsMajorEvent { get; set; } // Determines historical importance

        //[ForeignKey("Timeline")]
        public int TimelineId { get; set; }
        public Timeline Timeline { get; set; } = null!;

        //[ForeignKey("Location")]
        public int? LocationId { get; set; }
        public Location? Location { get; set; }

        public ICollection<Character> InvolvedCharacters { get; set; } = new List<Character>(); // ✅ People involved

        //[ForeignKey("Battle")]
        public int? BattleId { get; set; }
        public Battle? Battle { get; set; }


        //[ForeignKey("Folklore")]
        public int? FolkloreId { get; set; }
        public Folklore? Folklore { get; set; }
    }
}

