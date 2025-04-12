using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Nationality;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Military
{
    public class Army : ILoreEntity
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

        public bool IsStandingArmy { get; set; } // Permanent army or wartime force?
        public int Size { get; set; } // Total soldiers

        //[ForeignKey("City")]
        public int? CityId { get; set; }
        public City? City { get; set; }


        //[ForeignKey("MilitaryOrganization")]
        public int? MilitaryOrganizationId { get; set; }
        public MilitaryOrganization? MilitaryOrganization { get; set; }


        //[ForeignKey("Faction")]
        public int? FactionId { get; set; }
        public Faction? Faction { get; set; }


        public ICollection<MilitaryUnit> Units { get; set; } = new List<MilitaryUnit>(); // The units in this army
        public ICollection<Battle> Battles { get; set; } = new List<Battle>();
    }
}
