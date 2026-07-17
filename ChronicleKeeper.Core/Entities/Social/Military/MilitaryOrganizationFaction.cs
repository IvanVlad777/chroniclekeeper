namespace ChronicleKeeper.Core.Entities.Social.Military
{
    /// <summary>Join: MilitaryOrganization ↔ Faction. Composite PK, not a LoreEntity.</summary>
    public class MilitaryOrganizationFaction
    {
        public int MilitaryOrganizationId { get; set; }
        public virtual MilitaryOrganization? MilitaryOrganization { get; set; }

        public int FactionId { get; set; }
        public virtual Faction? Faction { get; set; }
    }
}
