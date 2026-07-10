using ChronicleKeeper.Core.Entities.Social.Structure;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    /// <summary>Join: Culture ↔ SocialClass (composite PK, not a LoreEntity).</summary>
    public class CultureSocialClass
    {
        public int CultureId { get; set; }
        public virtual Culture? Culture { get; set; }

        public int SocialClassId { get; set; }
        public virtual SocialClass? SocialClass { get; set; }
    }
}
