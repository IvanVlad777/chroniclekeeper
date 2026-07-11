using ChronicleKeeper.Core.Entities.Base;

namespace ChronicleKeeper.Core.Entities.Content.Book
{
    public class Chapter : LoreEntity
    {
        public int Order { get; set; }

        public int BookId { get; set; }
        public virtual Book? Book { get; set; }

        public virtual ICollection<Reference> References { get; set; } = new List<Reference>();
    }
}
