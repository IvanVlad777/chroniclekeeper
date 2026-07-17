using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Military
{
    public class MilitaryEquipment : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public string EquipmentType { get; set; } = string.Empty; // e.g., Weapon, Armor, Vehicle, Mount

        // M:N with MilitaryUnit (join entity)
        public virtual ICollection<MilitaryUnitEquipment> MilitaryUnits { get; set; } = new List<MilitaryUnitEquipment>();
    }
}
