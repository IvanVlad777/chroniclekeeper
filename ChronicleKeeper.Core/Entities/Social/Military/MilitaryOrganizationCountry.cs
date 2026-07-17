using ChronicleKeeper.Core.Entities.Geography;

namespace ChronicleKeeper.Core.Entities.Social.Military
{
    /// <summary>Join: MilitaryOrganization ↔ Country. Composite PK, not a LoreEntity.</summary>
    public class MilitaryOrganizationCountry
    {
        public int MilitaryOrganizationId { get; set; }
        public virtual MilitaryOrganization? MilitaryOrganization { get; set; }

        public int CountryId { get; set; }
        public virtual Country? Country { get; set; }
    }
}
