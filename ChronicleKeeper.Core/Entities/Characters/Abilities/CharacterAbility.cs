namespace ChronicleKeeper.Core.Entities.Characters.Abilities
{
    /// <summary>Join: Character ↔ Ability (composite PK, not a LoreEntity).</summary>
    public class CharacterAbility
    {
        public int CharacterId { get; set; }
        public virtual Character? Character { get; set; }

        public int AbilityId { get; set; }
        public virtual Ability? Ability { get; set; }
    }
}
