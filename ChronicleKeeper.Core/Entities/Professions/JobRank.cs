using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronicleKeeper.Core.Entities.Professions
{
    public class JobRank : ILoreEntity
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

        public string RankTitle { get; set; } = string.Empty; // Apprentice, Master, Grandmaster, etc.
        public int RankLevel { get; set; } // Higher number = higher rank (e.g., 1 = Beginner, 5 = Grandmaster)
        public string Responsibilities { get; set; } = string.Empty; // Tasks at this rank

        //[ForeignKey("Profession")]
        public int ProfessionId { get; set; }
        public Profession Profession { get; set; } = null!;
    }

}
