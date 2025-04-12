using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Content.Book
{
    public class Chapter
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Order { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;

        public ICollection<Reference> References { get; set; } = new List<Reference>();
    }
}
