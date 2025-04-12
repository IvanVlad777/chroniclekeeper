using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Structure;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Politics
{
    public class PrivilegeLaw : ILoreEntity
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
        
        public bool GrantsLegalImmunity { get; set; } // Nobles above the law?
        public bool GrantsLandOwnershipRights { get; set; } // If land is class-restricted
        public bool AllowsPrivateArmies { get; set; } // Can aristocrats have personal forces?

        //[ForeignKey("SocialClass")]
        public int SocialClassId { get; set; }
        private SocialClass SocialClass { get; set; } = null!;
    }
}
