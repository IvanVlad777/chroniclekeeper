using System.ComponentModel.DataAnnotations;
using static ChronicleKeeper.Core.Enums.SocietyEnums;

namespace ChronicleKeeper.Core.DTOs.GuildRank
{
    public class GuildRankDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int GuildId { get; set; }
        public string RankTitle { get; set; } = string.Empty;
        public RankLevel RankLevel { get; set; }
        public bool HasLeadershipAuthority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class GuildRankCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>Svijet ranga se izvodi iz ceha — ne šalje se WorldId.</summary>
        [Required]
        public int GuildId { get; set; }

        [StringLength(100)]
        public string RankTitle { get; set; } = string.Empty;

        public RankLevel RankLevel { get; set; }

        public bool HasLeadershipAuthority { get; set; }
    }

    public class GuildRankUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(100)]
        public string RankTitle { get; set; } = string.Empty;

        public RankLevel RankLevel { get; set; }

        public bool HasLeadershipAuthority { get; set; }
    }
}
