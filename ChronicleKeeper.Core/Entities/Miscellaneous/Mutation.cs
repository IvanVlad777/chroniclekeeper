using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography.Creatures;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using static ChronicleKeeper.Core.Enums.GlobalEnums;

namespace ChronicleKeeper.Core.Entities.Miscellaneous
{
    public class Mutation : LoreEntity
    {
        public MutationOrigin Origin { get; set; } // Radiation, Magic, GeneticExperiment, Disease, EvolutionaryAdaptation
        public MutationEffect Effect { get; set; } // Beneficial, Harmful, Mixed

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        // Optional pointer to the affected creature — SetNull.
        public int? MutantCreatureId { get; set; }
        public Creature? MutantCreature { get; set; }
    }
}
