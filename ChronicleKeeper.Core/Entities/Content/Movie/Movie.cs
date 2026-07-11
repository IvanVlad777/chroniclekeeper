namespace ChronicleKeeper.Core.Entities.Content.Movie
{
    public class Movie : Content
    {
        public string Director { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public int DurationMinutes { get; set; }

        public int? PrequelId { get; set; }
        public virtual Movie? Prequel { get; set; }
        public virtual ICollection<Movie> Sequels { get; set; } = new List<Movie>();
    }
}
