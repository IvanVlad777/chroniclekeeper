namespace ChronicleKeeper.Core.Entities.Geography.Climate
{
    /// <summary>Join: ClimateZone ↔ Season (composite PK, not a LoreEntity).</summary>
    public class ClimateZoneSeason
    {
        public int ClimateZoneId { get; set; }
        public virtual ClimateZone? ClimateZone { get; set; }

        public int SeasonId { get; set; }
        public virtual Season? Season { get; set; }
    }
}
