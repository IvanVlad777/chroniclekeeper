using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Characters;
//using ChronicleKeeper.Core.Entities.Social.Economy;
//using ChronicleKeeper.Core.Entities.Professions;
//using ChronicleKeeper.Core.Entities.Social.Cultures;
//using ChronicleKeeper.Core.Entities.Social.Politics;

namespace ChronicleKeeper.Core.Entities.Social.Structure
{
    public class SocialClass : LoreEntity
    {
        public bool IsNoble { get; set; } // True = Aristocracy, False = Commoner
        public bool IsMerchantClass { get; set; } // True = Economic elite
        public bool IsOutcast { get; set; } // True = Criminals, Exiles, Slaves
        public bool CanOwnLand { get; set; } // True if legally allowed to own property
        public bool CanHoldOffice { get; set; } // True if eligible for political roles
        public bool HasTaxExemptions { get; set; } // If this class is taxed differently

        public int? SocialHierarchyId { get; set; }
        public virtual SocialHierarchy? SocialHierarchy { get; set; }

        //public ICollection<Profession> TypicalProfessions { get; set; } = new List<Profession>(); // TODO: Uncomment when Profession entity is revived
        // NOTE: relation to Guild already exists via the GuildSocialClass join entity (Guild owns the nav) — target side gets no nav per convention.
        //public ICollection<Culture> Cultures { get; set; } = new List<Culture>(); // TODO: Uncomment when Culture entity is revived
        public ICollection<Character> Members { get; set; } = new List<Character>();
        public ICollection<PrivilegeLaw> PrivilegeLaws { get; set; } = new List<PrivilegeLaw>();
    }
}
