using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Nationality;
using ChronicleKeeper.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronicleKeeper.Core.Entities.Social.Military
{
    public class MilitaryRank : ILoreEntity
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

        public string RankTitle { get; set; } = string.Empty; // e.g., General, Captain, Lieutenant
        public int RankLevel { get; set; } // Higher numbers = higher rank (e.g., 5 = General, 1 = Private)

        //[ForeignKey("MilitaryUnit")]
        public int MilitaryUnitId { get; set; }
        public MilitaryUnit MilitaryUnit { get; set; } = null!;
    }
}
