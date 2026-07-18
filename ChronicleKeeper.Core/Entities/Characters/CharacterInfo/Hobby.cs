using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Characters.CharacterInfo
{
    public class Hobby : LoreEntity
    {
        public string Activity { get; set; } = string.Empty; // Painting, Hunting, Chess, Magic Studies

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public virtual ICollection<CharacterHobby> Practitioners { get; set; } = new List<CharacterHobby>();
    }
}
