using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronicleKeeper.Core.Enums
{
    public class GlobalEnums
    {
        public enum Rarity
        {
            Common = 1,
            Uncommon = 2,
            Rare = 3,
            Endemic = 4
        }

        public enum MutationOrigin
        {
            Radiation = 1,
            Magic = 2,
            GeneticExperiment = 3,
            Disease = 4,
            EvolutionaryAdaptation = 5
        }

        public enum MutationEffect
        {
            Beneficial = 1,
            Harmful = 2,
            Mixed = 3
        }
    }
}
