using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Economy;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Professions
{
    public class Apprenticeship : ILoreEntity
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

        public int DurationYears { get; set; } // Length of training in years
        public bool IsPaid { get; set; } // True if apprentices earn money while training
        public string SkillsTaught { get; set; } = string.Empty; // Skills learned

        //[ForeignKey("Profession")]
        public int ProfessionId { get; set; }
        public Profession Profession { get; set; } = null!;

        //[ForeignKey("Guild")]
        public int? GuildId { get; set; }
        public Guild? Guild { get; set; }

        //[ForeignKey("Corporation")]
        public int? CorporationId { get; set; } 
        public Corporation? Corporation { get; set; }

        //[ForeignKey("TradeSchool")]
        public int? TradeSchoolId { get; set; }
        public TradeSchool? TradeSchool { get; set; }
    }

}
