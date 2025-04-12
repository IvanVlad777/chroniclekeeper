using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.Entities.Base;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ChronicleKeeper.Core.Entities.Content
{
    public class Content
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public virtual ICollection<Reference> References { get; set; } = new List<Reference>();
    }
}
