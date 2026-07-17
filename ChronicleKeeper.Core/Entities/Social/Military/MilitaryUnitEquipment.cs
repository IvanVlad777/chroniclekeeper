namespace ChronicleKeeper.Core.Entities.Social.Military
{
    /// <summary>Join: MilitaryUnit ↔ MilitaryEquipment. Composite PK, not a LoreEntity.</summary>
    public class MilitaryUnitEquipment
    {
        public int MilitaryUnitId { get; set; }
        public virtual MilitaryUnit? MilitaryUnit { get; set; }

        public int MilitaryEquipmentId { get; set; }
        public virtual MilitaryEquipment? MilitaryEquipment { get; set; }
    }
}
