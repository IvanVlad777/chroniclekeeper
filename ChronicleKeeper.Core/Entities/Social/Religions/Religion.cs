using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.Social.Nationality;
using ChronicleKeeper.Core.Entities.Social.Cultures;
using System.ComponentModel.DataAnnotations.Schema;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.Interfaces;


namespace ChronicleKeeper.Core.Entities.Social.Religions
{
    public class Religion : ILoreEntity
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

        public string CoreBeliefs { get; set; } = string.Empty; // Summary of religious doctrine
        public string Practices { get; set; } = string.Empty; // Rituals, ceremonies, festivals
        public bool HasDeities { get; set; } // Whether the religion is theistic or non-theistic
        public bool IsStateReligion { get; set; } // If officially recognized by a government

        public ICollection<Deity> Deities { get; set; } = new List<Deity>(); // Gods worshipped in this religion
        public ICollection<ReligiousOrder> ReligiousOrders { get; set; } = new List<ReligiousOrder>();
        public ICollection<HolySite> HolySites { get; set; } = new List<HolySite>(); // Sacred locations
        public ICollection<ReligiousText> ReligiousTexts { get; set; } = new List<ReligiousText>();
        public ICollection<Myth> Myths { get; set; } = new List<Myth>(); // Myths of the religion
        public ICollection<Tradition> Traditions { get; set; } = new List<Tradition>();
        public ICollection<Culture> Cultures { get; set; } = new List<Culture>();

        public ICollection<Country> InCountries { get; set; } = new List<Country>(); // Countries where it's practiced
        public ICollection<City> InCities { get; set; } = new List<City>(); // Cities where it has influence
    }
}
