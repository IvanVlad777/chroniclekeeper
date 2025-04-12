using ChronicleKeeper.Core.Entities.Base;

namespace ChronicleKeeper.Core.Entities.Geography
{
    public class Continent : Location
    {
        //public ICollection<Region> Regions { get; set; } = new List<Region>();
        public string? ContinentSpecifics { get; set; }
    }
}
