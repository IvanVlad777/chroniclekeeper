using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography.Creatures;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Professions;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ChronicleKeeper.Core.Enums.GlobalEnums;

namespace ChronicleKeeper.Core.Entities.Miscellaneous
{
    public class Mutation : ILoreEntity
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
        public MutationOrigin Origin { get; set; } // Enum: Radiation, Magic, Experiment, etc.
        public MutationEffect Effect { get; set; } // Enum: Beneficial, Harmful, Mixed

        //[ForeignKey("MutantCreature")]
        public int? MutantCreatureId { get; set; }
        public Creature? MutantCreature { get; set; }
    }
}
