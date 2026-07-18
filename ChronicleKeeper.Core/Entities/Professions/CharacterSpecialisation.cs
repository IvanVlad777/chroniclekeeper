using ChronicleKeeper.Core.Entities.Characters;

namespace ChronicleKeeper.Core.Entities.Professions
{
    /// <summary>Join: Character ↔ Specialisation (composite PK, not a LoreEntity).</summary>
    public class CharacterSpecialisation
    {
        public int CharacterId { get; set; }
        public virtual Character? Character { get; set; }

        public int SpecialisationId { get; set; }
        public virtual Specialisation? Specialisation { get; set; }
    }
}
