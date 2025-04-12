namespace ChronicleKeeper.Core.Entities.Content.Movie
{
    public class Movie : Content
    {
        public string Director { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public int DurationMinutes { get; set; }
        public List<Movie> Sequels { get; set; } = new();
    }
}
