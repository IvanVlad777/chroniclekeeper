using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.Social.Politics;
using ChronicleKeeper.Core.Entities.Social.Religions;
using ChronicleKeeper.Core.Entities.Social.Cultures;
using System.ComponentModel.DataAnnotations.Schema;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.Interfaces;
using ChronicleKeeper.Core.Entities.Social.Structure;

namespace ChronicleKeeper.Core.Entities.Social.Nationality
{
    public class Nation : ILoreEntity
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

        public ICollection<PoliticalParty> PoliticalParties { get; set; } = new List<PoliticalParty>();

        // ✅ Social Structure & Culture
        public ICollection<Character> Characters { get; set; } = new List<Character>();
        public ICollection<Religion> StateReligions { get; set; } = new List<Religion>(); // Official religions
        public ICollection<Language> LanguagesSpoken { get; set; } = new List<Language>();
        public ICollection<Culture> Culture { get; set; } = new List<Culture>();

        //[ForeignKey("SocialHierarchy")]
        public int? SocialHierarchyId { get; set; }
        public SocialHierarchy? SocialHierarchy { get; set; }

        // ✅ Diplomacy & Trade
        //public ICollection<DiplomaticAgreement> DiplomaticAgreements { get; set; } = new List<DiplomaticAgreement>();

        // ✅ National Territory & Population
        public ICollection<City> Cities { get; set; } = new List<City>(); // ✅ Allow many-to-many relationship with cities
        public ICollection<Country> Countries { get; set; } = new List<Country>(); // ✅ Allow many-to-many relationship with countries

        public int Population { get; set; }
    }
}