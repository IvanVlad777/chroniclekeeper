using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Religions;
using ChronicleKeeper.Core.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Education
{
    public class ReligiousEducation : ILoreEntity
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

        public DateTime StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public bool Ordained { get; set; }

        //[ForeignKey("Character")]
        //public int CharacterId { get; set; }

        //[ForeignKey("ReligiousOrder")]
        public int ReligiousOrderId { get; set; }
        public ReligiousOrder ReligiousOrder { get; set; } = null!;

        //[ForeignKey("Religion")]
        public int ReligionId { get; set; }
        public Religion Religion { get; set; } = null!;

        //[ForeignKey("School")]
        //public int SchoolId { get; set; }
        //public School School { get; set; } = null!;

        //[ForeignKey("Religion")]
        //public int ReligionId { get; set; }
        //public Religion Religion { get; set; } = null!;
    }
}
