using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Military
{
    public class MilitaryUnit : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public string UnitType { get; set; } = string.Empty; // e.g., Infantry, Cavalry, Artillery, Naval Fleet
        public int Size { get; set; }
        public bool IsElite { get; set; }

        // Compositional child of an army — required FK, Cascade.
        public int BelongsToArmyId { get; set; }
        public virtual Army BelongsToArmy { get; set; } = null!;

        public virtual ICollection<MilitaryRank> Ranks { get; set; } = new List<MilitaryRank>();

        // M:N with MilitaryEquipment (join entity)
        public virtual ICollection<MilitaryUnitEquipment> Equipment { get; set; } = new List<MilitaryUnitEquipment>();
    }
}
