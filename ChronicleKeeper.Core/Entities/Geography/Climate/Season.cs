using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Geography.Climate
{
    public class Season : ILoreEntity
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

        //public int ClimateZoneId { get; set; }
        //public ClimateZone ClimateZone { get; set; } = null!;
        public ICollection<ClimateZone> ClimateZone { get; set; } = new List<ClimateZone>();

        [Range(1, 10000)]
        public int DurationInDays { get; set; } // Trajanje sezone u danima
        [Range(-100, 100)]
        public double TypicalTemperature { get; set; } // Prosječna temperatura tijekom sezone
        [Range(0, 10000)]
        public double TypicalPrecipitation { get; set; } // Prosječne padaline u sezoni
    }
}
