namespace ChronicleKeeper.Core.Entities.Content.Movie
{
    public class Series : Content
    {
        public string Creator { get; set; } = string.Empty;
        public int Seasons { get; set; }
        public ICollection<Episode> Episodes { get; set; } = new List<Episode>();
        public override ICollection<Reference> References => Episodes.SelectMany(ch => ch.References).Distinct().ToList();

    }
}
