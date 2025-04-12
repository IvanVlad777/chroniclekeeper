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
    public class GovernmentSystem : ILoreEntity
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

        // Osnovne karakteristike vladavine
        public bool IsDemocratic { get; set; } // True = Demokratski, False = Autoritarni
        public bool IsMonarchic { get; set; } // True = Monarhija
        public bool IsReligious { get; set; } // True = Teokracija

        // Razine centralizacije
        public bool IsFederal { get; set; } // Ako ima autonomne regije
        public bool IsCentralized { get; set; } // Ako je moć centralizirana
        // Connection to political ideology
        //[ForeignKey("PoliticalIdeology")]
        public int? PoliticalIdeologyId { get; set; }
        public PoliticalIdeology? PoliticalIdeology { get; set; }

        // Connection to political parties (supports multiparty or one-party states)
        public ICollection<PoliticalParty> PoliticalParties { get; set; } = new List<PoliticalParty>();

        // ✅ Izborni sustav
        public ElectionSystem ElectionSystem { get; set; } // Npr. "Direct Election", "Parliamentary", "Hereditary"

        // ✅ Stabilnost i ograničenja
        public ScaleLevel StabilityLevel { get; set; } // Low, Moderate, High
        public bool HasTermLimits { get; set; } // Postoje li ograničenja mandata?
        public int? MaxTermLength { get; set; } // Maksimalno trajanje mandata (ako postoji ograničenje)
        
        public ICollection<Country> Countries { get; set; } = new List<Country>();
        public ICollection<City> Cities { get; set; } = new List<City>();
        //public ICollection<Nation> Nations { get; set; } = new List<Nation>();
    }
}
