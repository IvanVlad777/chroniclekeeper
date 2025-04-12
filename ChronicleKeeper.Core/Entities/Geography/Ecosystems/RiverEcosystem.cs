using ChronicleKeeper.Core.Entities.Geography.Ecosystems;
using ChronicleKeeper.Core.Entities.Professions;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Geography.Ecosystems
{
    public class RiverEcosystem : WaterEcosystem
    {
        public double Length { get; set; } // in kilometers

        //[ForeignKey("SourceLocation")]
        //public int? SourceLocationId { get; set; }
        //public Location? SourceLocation { get; set; }

        //[ForeignKey("MouthLocation")]
        //public int? MouthLocationId { get; set; }
        //public Location? MouthLocation { get; set; }
    }
}
