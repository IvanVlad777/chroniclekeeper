using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronicleKeeper.Core.Enums
{
    public class EcosystemEnums
    {
        public enum ForestType
        {
            Rainforest = 1,
            Boreal = 2,
            Temperate = 3,
            Mangrove = 4,
            Redwood = 5
        }

        public enum DesertType
        {
            Sandy = 1,
            Rocky = 2,
            Ice = 3
        }

        public enum CaveType
        {
            Limestone = 1,
            LavaTube = 2,
            IceCave = 3,
            SeaCave = 4
        }

        public enum GrasslandType
        {
            Prairie = 1,      // Tall grass, temperate climate
            Steppe = 2,       // Short grass, semi-arid
            Savannah = 3,     // Scattered trees, tropical
            Meadow = 4,       // Seasonal wildflowers, wet-dry cycle
            Heathland = 5,    // Shrubby, acidic soil, cool climate
            Pampa = 6,        // South American, fertile lowlands
            Llanos = 7,       // South American, tropical floodplains
            Veld = 8,         // African, mixed grasslands
            TundraGrassland = 9 // Cold-adapted grasslands in Arctic/Alpine
        }
    }
}
