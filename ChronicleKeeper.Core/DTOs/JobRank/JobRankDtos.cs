using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.JobRank
{
    public class JobRankDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int WorldId { get; set; }
        public int ProfessionId { get; set; }
        public string RankTitle { get; set; } = string.Empty;
        public int RankLevel { get; set; }
        public string Responsibilities { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class JobRankCreateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>Svijet ranga se izvodi iz zanimanja — ne šalje se WorldId.</summary>
        [Required]
        public int ProfessionId { get; set; }

        [StringLength(100)]
        public string RankTitle { get; set; } = string.Empty;

        public int RankLevel { get; set; }

        [StringLength(1000)]
        public string Responsibilities { get; set; } = string.Empty;
    }

    public class JobRankUpdateDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "Description cannot exceed 4000 characters")]
        public string Description { get; set; } = string.Empty;

        [StringLength(100)]
        public string RankTitle { get; set; } = string.Empty;

        public int RankLevel { get; set; }

        [StringLength(1000)]
        public string Responsibilities { get; set; } = string.Empty;
    }
}
