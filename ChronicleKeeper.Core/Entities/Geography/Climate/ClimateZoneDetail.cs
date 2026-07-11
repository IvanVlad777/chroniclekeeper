namespace ChronicleKeeper.Core.Entities.Geography.Climate
{
    /// <summary>Join: ClimateZone ↔ ClimateDetail (composite PK, not a LoreEntity).</summary>
    public class ClimateZoneDetail
    {
        public int ClimateZoneId { get; set; }
        public virtual ClimateZone? ClimateZone { get; set; }

        public int ClimateDetailId { get; set; }
        public virtual ClimateDetail? ClimateDetail { get; set; }
    }
}
