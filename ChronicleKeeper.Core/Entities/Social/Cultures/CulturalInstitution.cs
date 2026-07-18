using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class CulturalInstitution : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public string InstitutionType { get; set; } = string.Empty; // Theater, Museum, Art Gallery
        public bool IsGovernmentFunded { get; set; } // If state-controlled

        // Optional culture link — SetNull. (Was mis-named CountryId in the dormant scaffold.)
        public int? CultureId { get; set; }
        public virtual Culture? Culture { get; set; }

        // Optional city link — SetNull. (City is mapped but has no API/picker yet.)
        public int? CityId { get; set; }
        public virtual City? City { get; set; }

        public virtual ICollection<CulturalInstitutionArtist> NotableArtists { get; set; } = new List<CulturalInstitutionArtist>();
    }
}
