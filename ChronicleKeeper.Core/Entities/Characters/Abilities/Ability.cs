using ChronicleKeeper.Core.Entities.Base;
using System.ComponentModel.DataAnnotations;
using static ChronicleKeeper.Core.Enums.EquipmentEnums;

namespace ChronicleKeeper.Core.Entities.Characters.Abilities
{
    public class Ability : LoreEntity
    {
        //public virtual History? History { get; set; } // TODO: Uncomment when History entity is revived

        [Required]
        public AbilityType Type { get; set; }

        public virtual ICollection<AbilityLevel> AbilityLevels { get; set; } = new List<AbilityLevel>();

        public virtual ICollection<CharacterAbility> Characters { get; set; } = new List<CharacterAbility>();
    }
}
