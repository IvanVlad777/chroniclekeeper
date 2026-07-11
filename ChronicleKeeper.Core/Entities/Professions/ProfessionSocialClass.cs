using ChronicleKeeper.Core.Entities.Social.Structure;

namespace ChronicleKeeper.Core.Entities.Professions
{
    /// <summary>Join: Profession ↔ SocialClass (composite PK, not a LoreEntity).</summary>
    public class ProfessionSocialClass
    {
        public int ProfessionId { get; set; }
        public virtual Profession? Profession { get; set; }

        public int SocialClassId { get; set; }
        public virtual SocialClass? SocialClass { get; set; }
    }
}
