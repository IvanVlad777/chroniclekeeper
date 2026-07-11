using ChronicleKeeper.Core.Entities.Base;

namespace ChronicleKeeper.Core.Entities.Content
{
    /// <summary>TPH root for Article/Book/Comic/Movie/Series — discriminated by "ContentType".</summary>
    public abstract class Content : LoreEntity
    {
        public virtual ICollection<Reference> References { get; set; } = new List<Reference>();
    }
}
