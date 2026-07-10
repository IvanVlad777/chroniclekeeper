using ChronicleKeeper.Core.Entities.Social;

namespace ChronicleKeeper.Core.Entities.Social.Politics
{
    /// <summary>Join: PoliticalParty ↔ Faction (composite PK, not a LoreEntity).</summary>
    public class PoliticalPartyFaction
    {
        public int PoliticalPartyId { get; set; }
        public virtual PoliticalParty? PoliticalParty { get; set; }

        public int FactionId { get; set; }
        public virtual Faction? Faction { get; set; }
    }
}
