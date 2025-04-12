using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronicleKeeper.Core.Entities.Content.Book
{
    public class Comic : Content
    {
        public string Author { get; set; } = string.Empty;
        public int IssueNumber { get; set; }
    }
}
