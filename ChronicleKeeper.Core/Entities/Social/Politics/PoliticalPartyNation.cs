using ChronicleKeeper.Core.Entities.Social.Nationality;

namespace ChronicleKeeper.Core.Entities.Social.Politics
{
    /// <summary>Join: PoliticalParty ↔ Nation (composite PK, not a LoreEntity).</summary>
    public class PoliticalPartyNation
    {
        public int PoliticalPartyId { get; set; }
        public virtual PoliticalParty? PoliticalParty { get; set; }

        public int NationId { get; set; }
        public virtual Nation? Nation { get; set; }
    }
}
