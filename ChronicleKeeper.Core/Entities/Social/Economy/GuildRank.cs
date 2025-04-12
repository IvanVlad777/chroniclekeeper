using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ChronicleKeeper.Core.Enums.SocietyEnums;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class GuildRank : ILoreEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public virtual History? History { get; set; }

        public string RankTitle { get; set; } = string.Empty; // Master, Journeyman, Apprentice
        public RankLevel RankLevel { get; set; } // 1 = Master, 2 = Journeyman, 3 = Apprentice
        public bool HasLeadershipAuthority { get; set; } // True for Guild Masters

        //[ForeignKey("Guild")]
        public int GuildId { get; set; }
        public Guild Guild { get; set; } = null!;
    }
}
