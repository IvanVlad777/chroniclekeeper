namespace ChronicleKeeper.Core.Entities.Geography.Climate
{
    /// <summary>Join: Location ↔ ClimateZone (composite PK, not a LoreEntity).</summary>
    public class LocationClimateZone
    {
        public int LocationId { get; set; }
        public virtual Geography.Location? Location { get; set; }

        public int ClimateZoneId { get; set; }
        public virtual ClimateZone? ClimateZone { get; set; }
    }
}
