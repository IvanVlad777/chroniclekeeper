using ChronicleKeeper.Core.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Professions
{
    public class JobRank : LoreEntity
    {
        public string RankTitle { get; set; } = string.Empty; // Apprentice, Master, Grandmaster, etc.
        public int RankLevel { get; set; } // Higher number = higher rank (e.g., 1 = Beginner, 5 = Grandmaster)
        public string Responsibilities { get; set; } = string.Empty; // Tasks at this rank

        [Required]
        public int ProfessionId { get; set; }
        public virtual Profession Profession { get; set; } = null!;
    }
}
