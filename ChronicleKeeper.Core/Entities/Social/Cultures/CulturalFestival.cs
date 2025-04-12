using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class CulturalFestival : ILoreEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public virtual History? History { get; set; }

        public DateTime StartDate { get; set; }
        public int DurationDays { get; set; } // Festival length
        public string Activities { get; set; } = string.Empty; // Events, customs, dances
        public bool IsNationalHoliday { get; set; } // Whether the festival is an official holiday

        //[ForeignKey("Location")]
        public int? LocationId { get; set; }
        public Location? Location { get; set; }


        //[ForeignKey("Culture")]
        public int CultureId { get; set; }
        public Culture Culture { get; set; } = null!;
    }

}
