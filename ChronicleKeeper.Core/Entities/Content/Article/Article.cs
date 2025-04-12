using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronicleKeeper.Core.Entities.Content.Article
{
    public class Article : Content
    {
        public string Source { get; set; } = string.Empty;
        public DateTime PublishDate { get; set; }
    }
}
