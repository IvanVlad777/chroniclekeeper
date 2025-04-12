using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class ArtForm : ILoreEntity
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

        public string Type { get; set; } = string.Empty; // Music, Painting, Sculpture, Dance
        public string NotableArtists { get; set; } = string.Empty; // Famous figures in this art form
        public string HistoricalInfluences { get; set; } = string.Empty;

        //[ForeignKey("Culture")]
        public int CultureId { get; set; }
        public Culture Culture { get; set; } = null!;
    }

}
