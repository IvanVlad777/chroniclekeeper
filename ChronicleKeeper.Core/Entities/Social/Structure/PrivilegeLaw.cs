using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Structure
{
    public class PrivilegeLaw : LoreEntity
    {
        public bool GrantsLegalImmunity { get; set; }        // Nobles above the law?
        public bool GrantsLandOwnershipRights { get; set; }  // If land is class-restricted
        public bool AllowsPrivateArmies { get; set; }        // Can aristocrats have personal forces?

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        // Compositional child of a SocialClass — required FK, Cascade (a privilege law is
        // meaningless without its class).
        public int SocialClassId { get; set; }
        public SocialClass SocialClass { get; set; } = null!;
    }
}
