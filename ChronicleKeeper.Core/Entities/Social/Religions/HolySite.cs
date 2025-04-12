using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Religions
{
    public class HolySite : ILoreEntity
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

        public string Significance { get; set; } = string.Empty; // Why it's sacred (miracles, divine events)
        public bool IsPilgrimageDestination { get; set; } // If people travel here for religious reasons

        //[ForeignKey("Religion")]
        public int ReligionId { get; set; }
        public Religion Religion { get; set; } = null!;

        //[ForeignKey("Deity")]
        public int? DeityId { get; set; }
        public Deity? Deity { get; set; }

        //[ForeignKey("Location")]
        public int LocationId { get; set; }
        public Location? Location { get; set; }
    }

}
