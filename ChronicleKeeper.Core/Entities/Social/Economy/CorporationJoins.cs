using ChronicleKeeper.Core.Entities.Professions;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    /// <summary>Join: Corporation ↔ Faction (composite PK, not a LoreEntity).</summary>
    public class CorporationFaction
    {
        public int CorporationId { get; set; }
        public virtual Corporation? Corporation { get; set; }

        public int FactionId { get; set; }
        public virtual Faction? Faction { get; set; }
    }

    /// <summary>Join: Corporation ↔ Profession (composite PK, not a LoreEntity).</summary>
    public class CorporationProfession
    {
        public int CorporationId { get; set; }
        public virtual Corporation? Corporation { get; set; }

        public int ProfessionId { get; set; }
        public virtual Profession? Profession { get; set; }
    }
}
