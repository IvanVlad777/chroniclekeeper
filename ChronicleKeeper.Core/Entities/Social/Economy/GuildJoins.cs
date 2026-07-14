using ChronicleKeeper.Core.Entities.Professions;
using ChronicleKeeper.Core.Entities.Social.Structure;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    /// <summary>Join: Guild ↔ Faction (composite PK, not a LoreEntity).</summary>
    public class GuildFaction
    {
        public int GuildId { get; set; }
        public virtual Guild? Guild { get; set; }

        public int FactionId { get; set; }
        public virtual Faction? Faction { get; set; }
    }

    /// <summary>Join: Guild ↔ Profession (composite PK, not a LoreEntity).</summary>
    public class GuildProfession
    {
        public int GuildId { get; set; }
        public virtual Guild? Guild { get; set; }

        public int ProfessionId { get; set; }
        public virtual Profession? Profession { get; set; }
    }

    /// <summary>Join: Guild ↔ SocialClass (composite PK, not a LoreEntity).</summary>
    public class GuildSocialClass
    {
        public int GuildId { get; set; }
        public virtual Guild? Guild { get; set; }

        public int SocialClassId { get; set; }
        public virtual SocialClass? SocialClass { get; set; }
    }
}
