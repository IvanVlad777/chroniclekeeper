using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Military
{
    public class Battle : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        // Fictional in-world date: plain string stopgap (like TimelineEvent.Date / Character dates).
        public string? BattleDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Outcome { get; set; } = string.Empty;

        // M:N with Army (join entity)
        public virtual ICollection<ArmyBattle> ParticipatingArmies { get; set; } = new List<ArmyBattle>();
    }
}
