using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Economy;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;
using static ChronicleKeeper.Core.Enums.SocietyEnums;

namespace ChronicleKeeper.Core.Entities.Social.Politics
{
    public class LegalSystem : ILoreEntity
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

        public string Laws { get; set; } = string.Empty; // Overview of legal structure

        public ICollection<City> Cities { get; set; } = new List<City>();
        public ICollection<Country> Countries { get; set; } = new List<Country>();
        public ICollection<Guild> Guilds { get; set; } = new List<Guild>();

        public ScaleLevel JudicialIndependence { get; set; } // Low, Moderate, High
        public PunishmentMethods PunishmentMethods { get; set; } // Npr. Death Penalty, Forced Labor, Fines
    }
}
