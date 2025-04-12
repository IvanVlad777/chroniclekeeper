using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Social.Economy;
using ChronicleKeeper.Core.Entities.Professions;
using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.Social.Politics;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Structure
{
    public class SocialClass : ILoreEntity
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

        public bool IsNoble { get; set; } // True = Aristocracy, False = Commoner
        public bool IsMerchantClass { get; set; } // True = Economic elite
        public bool IsOutcast { get; set; } // True = Criminals, Exiles, Slaves
        public bool CanOwnLand { get; set; } // True if legally allowed to own property
        public bool CanHoldOffice { get; set; } // True if eligible for political roles
        public bool HasTaxExemptions { get; set; } // If this class is taxed differently

        //[ForeignKey("SocialHierarchy")]
        public int? SocialHierarchyId { get; set; }
        private SocialClass? SocialHierarchy { get; set; }

        public ICollection<Profession> TypicalProfessions { get; set; } = new List<Profession>(); // Jobs held by this class
        public ICollection<Guild> Guilds { get; set; } = new List<Guild>(); // Organizations tied to the class
        public ICollection<Culture> Cultures { get; set; } = new List<Culture>(); // Cultures that influence this class
        public ICollection<Character> Members { get; set; } = new List<Character>();
        public ICollection<PrivilegeLaw> PrivilegeLaws { get; set; } = new List<PrivilegeLaw>();
    }
}
