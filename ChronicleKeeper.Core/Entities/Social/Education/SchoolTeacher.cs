using ChronicleKeeper.Core.Entities.Characters;

namespace ChronicleKeeper.Core.Entities.Social.Education
{
    /// <summary>Join: School ↔ Character (teacher role; composite PK, not a LoreEntity).</summary>
    public class SchoolTeacher
    {
        public int SchoolId { get; set; }
        public virtual School? School { get; set; }

        public int CharacterId { get; set; }
        public virtual Character? Character { get; set; }
    }
}
