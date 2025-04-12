using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Geography.Ecosystems
{
    public class MountainEcosystem : Ecosystem
    {
        public double MaxElevation { get; set; } // in meters
        public double Prominence { get; set; } // in meters

        //[ForeignKey("MountainRange")]
        //public int MountainRangeId { get; set; }
        //public MountainRange? MountainRange { get; set; }
    }
}
