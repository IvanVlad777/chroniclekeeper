using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Religions
{
    public class ReligiousOrder : ILoreEntity
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

        public string OrderType { get; set; } = string.Empty; // Clergy, Monastic, Cult, Sect
        public string Beliefs { get; set; } = string.Empty; // Unique ideology of the order
        public bool IsMilitant { get; set; } // Whether the order is military-focused
        public bool IsSecretive { get; set; } // Whether it operates in secrecy

        //[ForeignKey("Religion")]
        public int? ReligionId { get; set; }
        public Religion? Religion { get; set; }

        public ICollection<ReligiousEducation> ClergyTraining { get; set; } = new List<ReligiousEducation>();

        public ICollection<Deity> Deities { get; set; } = new List<Deity>();

        //public ICollection<Country> OperatesInCountries { get; set; } = new List<Country>(); // ✅ Countries where it operates
        //public ICollection<City> OperatesInCities { get; set; } = new List<City>(); // ✅ Cities where it has influence
        public ICollection<Faction> Factions { get; set; } = new List<Faction>();

    }

}
