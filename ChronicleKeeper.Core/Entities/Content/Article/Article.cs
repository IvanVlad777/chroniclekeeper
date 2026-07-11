namespace ChronicleKeeper.Core.Entities.Content.Article
{
    public class Article : Content
    {
        public string Source { get; set; } = string.Empty;
        public DateTime PublishDate { get; set; }
    }
}
