namespace ChronicleKeeper.Core.Entities.Characters.CharacterInfo
{
    /// <summary>Join: Character ↔ Hobby (composite PK, not a LoreEntity).</summary>
    public class CharacterHobby
    {
        public int CharacterId { get; set; }
        public virtual Character? Character { get; set; }

        public int HobbyId { get; set; }
        public virtual Hobby? Hobby { get; set; }
    }
}
