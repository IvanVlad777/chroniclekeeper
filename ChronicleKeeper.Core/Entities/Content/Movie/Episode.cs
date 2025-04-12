using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Content.Movie
{
    public class Episode
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Order { get; set; }
        public int Season { get; set; }
        public int SeriesId { get; set; }
        public Series Series { get; set; } = null!;
        public ICollection<Reference> References { get; set; } = new List<Reference>();
    }
}
