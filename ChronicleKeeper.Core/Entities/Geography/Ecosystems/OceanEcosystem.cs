using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ChronicleKeeper.Core.Entities.Geography.Ecosystems
{
    public class OceanEcosystem : WaterEcosystem
    {
        [NotMapped]
        public IEnumerable<SeaEcosystem> Seas => SubLocations.OfType<SeaEcosystem>();
    }
}
