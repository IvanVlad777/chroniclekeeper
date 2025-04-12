using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography.Ecosystems;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Miscellaneous;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ChronicleKeeper.Core.Enums.CreatureEnums;


namespace ChronicleKeeper.Core.Entities.Geography.Creatures
{
    public abstract class Creature : ILoreEntity
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
        public int? ParentCreatureId { get; set; }
        [ForeignKey("ParentCreatureId")]
        public virtual Creature? ParentCreature { get; set; }
        public virtual ICollection<Creature> Subspecies { get; set; } = new List<Creature>();

        public ICollection<Creature> SymbioticPartners { get; set; } = new List<Creature>();
        public ICollection<Creature> Prey { get; set; } = new List<Creature>();

        [InverseProperty("Prey")]
        public ICollection<Creature> Predators { get; set; } = new List<Creature>();

        public CreatureType Type { get; set; } // Enum (Human, Animal, Mythical, etc.)
        public double AverageLifespan { get; set; } // Lifespan in years
        public double Height { get; set; } // Average height in meters
        public double Weight { get; set; } // Average weight in kg

        public bool IsSentient { get; set; } // True = Can reason, False = Purely instinctual
        public bool IsArtificial { get; set; } = false; // True if a robot, AI, cyborg, etc.
        public ArtificialOrigin? ArtificialOrigin { get; set; } = null; // Enum: AI, Golem, Cyborg, etc.

        public ICollection<Ecosystem> Habitants { get; set; } = new List<Ecosystem>(); // Where it lives
        public ICollection<City> CitiesItInhabits { get; set; } = new List<City>(); // Where it lives
        public ICollection<Mutation> Mutations { get; set; } = new List<Mutation>();
    }

}
