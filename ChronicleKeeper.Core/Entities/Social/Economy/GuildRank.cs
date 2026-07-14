using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using System.ComponentModel.DataAnnotations;
using static ChronicleKeeper.Core.Enums.SocietyEnums;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class GuildRank : LoreEntity
    {
        public string RankTitle { get; set; } = string.Empty; // Master, Journeyman, Apprentice
        public RankLevel RankLevel { get; set; }
        public bool HasLeadershipAuthority { get; set; } // True for Guild Masters

        [Required]
        public int GuildId { get; set; }
        public virtual Guild Guild { get; set; } = null!;

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }
    }
}
