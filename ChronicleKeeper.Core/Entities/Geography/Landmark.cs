using ChronicleKeeper.Core.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Geography
{
    public class Landmark : Location
    {
        //[ForeignKey("Location")]
        //public int LocationId { get; set; }
        //public Location Location { get; set; } = null!;

        public string LandmarkType { get; set; } = string.Empty;  // npr. povijesni spomenik, zgrada, park
    }
}
