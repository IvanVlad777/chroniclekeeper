namespace ChronicleKeeper.Core.Entities.Content.Book
{
    public class Book : Content
    {
        public string Author { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }

        public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
    }
}
