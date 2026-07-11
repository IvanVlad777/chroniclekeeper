using ChronicleKeeper.Core.Entities.Base;

namespace ChronicleKeeper.Core.Entities.Content.Movie
{
    public class Episode : LoreEntity
    {
        public int Order { get; set; }
        public int Season { get; set; }

        public int SeriesId { get; set; }
        public virtual Series? Series { get; set; }

        public virtual ICollection<Reference> References { get; set; } = new List<Reference>();
    }
}
