using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ChronicleKeeper.Core.Entities.Geography.Ecosystems
{
    public class MountainRange : Ecosystem
    {
        public double MountainRangeLength { get; set; } // in kilometers, named to avoid colliding with RiverEcosystem.RiverLength

        [NotMapped]
        public IEnumerable<MountainEcosystem> Mountains => SubLocations.OfType<MountainEcosystem>();
    }
}
