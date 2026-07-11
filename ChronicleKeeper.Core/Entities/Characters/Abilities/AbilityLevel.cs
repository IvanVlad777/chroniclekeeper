using ChronicleKeeper.Core.Entities.Base;
using System.ComponentModel.DataAnnotations;
using static ChronicleKeeper.Core.Enums.EquipmentEnums;

namespace ChronicleKeeper.Core.Entities.Characters.Abilities
{
    public class AbilityLevel : LoreEntity
    {
        //public virtual History? History { get; set; } // TODO: Uncomment when History entity is revived

        [Required]
        public AbilityRank Rank { get; set; }

        public int AbilityId { get; set; }
        public virtual Ability? Ability { get; set; }
    }
}
