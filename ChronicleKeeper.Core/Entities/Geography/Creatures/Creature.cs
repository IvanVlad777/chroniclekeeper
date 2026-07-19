using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using System.ComponentModel.DataAnnotations.Schema;
using static ChronicleKeeper.Core.Enums.CreatureEnums;
//using ChronicleKeeper.Core.Entities.Geography.Ecosystems;
//using ChronicleKeeper.Core.Entities.Miscellaneous;


namespace ChronicleKeeper.Core.Entities.Geography.Creatures
{
    public abstract class Creature : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public int? ParentCreatureId { get; set; }
        [ForeignKey("ParentCreatureId")]
        public virtual Creature? ParentCreature { get; set; }
        public virtual ICollection<Creature> Subspecies { get; set; } = new List<Creature>();

        public CreatureType Type { get; set; } // Enum (Sapient, Animal, Plant, Fungus, Spiritual, Mythical...)
        public double AverageLifespan { get; set; } // Lifespan in years
        public double Height { get; set; } // Average height in meters
        public double Weight { get; set; } // Average weight in kg

        public bool IsSentient { get; set; } // True = Can reason, False = Purely instinctual
        public bool IsArtificial { get; set; } = false; // True if a robot, AI, cyborg, etc.
        public ArtificialOrigin? ArtificialOrigin { get; set; } = null; // Enum: AI, Golem, Cyborg, etc.

        public virtual ICollection<CreatureCity> CitiesItInhabits { get; set; } = new List<CreatureCity>(); // Where it lives
        public virtual ICollection<CreatureEcosystem> Habitants { get; set; } = new List<CreatureEcosystem>(); // Ecosystems it lives in

        // TODO: Uncomment when Mutation entity is revived
        public ICollection<Miscellaneous.Mutation> Mutations { get; set; } = new List<Miscellaneous.Mutation>();
        // Self-referencing many-to-many (join entities in CreatureSelfLinks.cs). Navs are typed as
        // the join entity, not Creature directly (the join-entity convention). SymbioticPartners and
        // Prey are the owner (write) side; Predators is the reverse read side of CreaturePredation.
        public virtual ICollection<CreatureSymbiosis> SymbioticPartners { get; set; } = new List<CreatureSymbiosis>();
        public virtual ICollection<CreaturePredation> Prey { get; set; } = new List<CreaturePredation>();
        public virtual ICollection<CreaturePredation> Predators { get; set; } = new List<CreaturePredation>();
    }

}
