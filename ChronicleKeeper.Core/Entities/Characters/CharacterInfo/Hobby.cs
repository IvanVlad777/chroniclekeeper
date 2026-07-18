using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Characters.CharacterInfo
{
    public class Hobby : LoreEntity
    {
        public string Activity { get; set; } = string.Empty; // Painting, Hunting, Chess, Magic Studies

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        // Character M:N — deferred to the R5 cross-links round (defer bucket for all Character
        // many-to-many collections). TODO: Uncomment when Character↔Hobby join is revived.
        //public ICollection<Character> Practitioners { get; set; } = new List<Character>();
    }
}
