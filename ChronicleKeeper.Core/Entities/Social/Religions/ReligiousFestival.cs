using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Religions
{
    public class ReligiousFestival : ILoreEntity
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
        public int DurationDays { get; set; } // Length of festival
        public string Traditions { get; set; } = string.Empty; // Rituals and celebrations
        public bool IsPilgrimageEvent { get; set; } // If travel to a holy site is part of the festival

        //[ForeignKey("Religion")]
        public int ReligionId { get; set; }
        private Religion Religion { get; set; } = null!;

        //[ForeignKey("HolySite")]
        public int? HolySiteId { get; set; }
        private HolySite? HolySite { get; set; }


        //public ICollection<Country> CelebratedInCountries { get; set; } = new List<Country>(); // ✅ Countries where it's observed
        //public ICollection<City> CelebratedInCities { get; set; } = new List<City>(); // ✅ Cities where it's observed
    }

}
