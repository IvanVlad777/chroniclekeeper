using ChronicleKeeper.Core.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Geography
{
    public class District : Location
    {
        //[ForeignKey("City")]
        //public int CityId { get; set; }
        //public City City { get; set; } = null!;

        public string DistrictType { get; set; } = string.Empty;  // npr. stambeni, poslovni, industrijski

    }
}
