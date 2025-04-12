using ChronicleKeeper.Core.Entities.HistoryTimelines;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronicleKeeper.Core.Interfaces
{
    internal interface ILoreEntity
    {
        [Key]
        int Id { get; set; }
        [Required]
        string Name { get; set; }
        string Description { get; set; }
        [Required]
        DateTime CreatedAt { get; set; }
        [Required]
        DateTime UpdatedAt { get; set; }
        History? History { get; set; }
    }
}
