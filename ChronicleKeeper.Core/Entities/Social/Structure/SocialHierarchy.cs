using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Entities.Social.Nationality;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Social.Structure
{
    public class SocialHierarchy : ILoreEntity
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

        public bool IsCasteSystem { get; set; } // True = Fixed classes, False = Social mobility
        public bool AllowsUpwardMobility { get; set; } // Can lower classes rise?
        public bool AllowsIntermarriage { get; set; } // Can nobles marry commoners?
        public bool EnforcesLegalSeparation { get; set; } // True if laws divide classes

        public ICollection<SocialClass> Classes { get; set; } = new List<SocialClass>();

        public ICollection<Nation> Nations { get; set; } = new List<Nation>(); // Cultures that influence this class

    }
}
