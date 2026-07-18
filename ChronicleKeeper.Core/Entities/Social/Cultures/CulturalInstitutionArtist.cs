using ChronicleKeeper.Core.Entities.Characters;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    /// <summary>Join: CulturalInstitution ↔ Character (notable artist; composite PK, not a LoreEntity).</summary>
    public class CulturalInstitutionArtist
    {
        public int CulturalInstitutionId { get; set; }
        public virtual CulturalInstitution? CulturalInstitution { get; set; }

        public int CharacterId { get; set; }
        public virtual Character? Character { get; set; }
    }
}
