using ChronicleKeeper.Core.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Professions
{
    public class Apprenticeship : LoreEntity
    {
        public int DurationYears { get; set; } // Length of training in years
        public bool IsPaid { get; set; } // True if apprentices earn money while training
        public string SkillsTaught { get; set; } = string.Empty; // Skills learned

        [Required]
        public int ProfessionId { get; set; }
        public virtual Profession Profession { get; set; } = null!;

        //public int? GuildId { get; set; } // TODO: Uncomment when Guild entity is revived
        //public virtual Guild? Guild { get; set; }

        //public int? CorporationId { get; set; } // TODO: Uncomment when Corporation entity is revived
        //public virtual Corporation? Corporation { get; set; }

        public int? TradeSchoolId { get; set; }
        public virtual TradeSchool? TradeSchool { get; set; }
    }
}
