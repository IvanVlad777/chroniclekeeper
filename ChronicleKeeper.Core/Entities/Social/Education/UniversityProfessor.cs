using ChronicleKeeper.Core.Entities.Characters;

namespace ChronicleKeeper.Core.Entities.Social.Education
{
    /// <summary>Join: University ↔ Character (professor role; composite PK, not a LoreEntity).</summary>
    public class UniversityProfessor
    {
        public int UniversityId { get; set; }
        public virtual University? University { get; set; }

        public int CharacterId { get; set; }
        public virtual Character? Character { get; set; }
    }
}
