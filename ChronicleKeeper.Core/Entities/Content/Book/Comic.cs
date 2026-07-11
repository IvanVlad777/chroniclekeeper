namespace ChronicleKeeper.Core.Entities.Content.Book
{
    public class Comic : Content
    {
        public string Author { get; set; } = string.Empty;
        public int IssueNumber { get; set; }
    }
}
