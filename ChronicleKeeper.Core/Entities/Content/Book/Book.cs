namespace ChronicleKeeper.Core.Entities.Content.Book
{
    public class Book : Content
    {
        public string Author { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
        public override ICollection<Reference> References => Chapters.SelectMany(ch => ch.References).Distinct().ToList();
    }
}
