using ChronicleKeeper.Core.Entities.Characters;

namespace ChronicleKeeper.Core.Entities.Social.Education
{
    /// <summary>Join: Library ↔ Character (scholar role; composite PK, not a LoreEntity).</summary>
    public class LibraryScholar
    {
        public int LibraryId { get; set; }
        public virtual Library? Library { get; set; }

        public int CharacterId { get; set; }
        public virtual Character? Character { get; set; }
    }
}
