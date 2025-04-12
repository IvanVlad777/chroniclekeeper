using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Nationality;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ChronicleKeeper.Core.Enums.SocietyEnums;

namespace ChronicleKeeper.Core.Entities.Social.Politics
{
    public class PoliticalParty : ILoreEntity
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

        public string IdeologyDescription { get; set; } = string.Empty;

        //[ForeignKey("PoliticalIdeology")]
        public int PoliticalIdeologyId { get; set; }
        public PoliticalIdeology PoliticalIdeology { get; set; } = null!;


        public ICollection<Country> Countries { get; set; } = new List<Country>();
        public ICollection<City> Cities { get; set; } = new List<City>();
        public ICollection<Faction> Factions { get; set; } = new List<Faction>();

        public ICollection<Nation> Nations { get; set; } = new List<Nation>();

        public ScaleLevel InfluenceLevel { get; set; } // Low, Moderate, High
        public bool IsBanned { get; set; } // Ako je stranka ilegalna
    }

}
