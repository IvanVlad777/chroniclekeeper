namespace ChronicleKeeper.Core.Entities.Social.Military
{
    /// <summary>Join: Army ↔ Battle (participating armies). Composite PK, not a LoreEntity.</summary>
    public class ArmyBattle
    {
        public int ArmyId { get; set; }
        public virtual Army? Army { get; set; }

        public int BattleId { get; set; }
        public virtual Battle? Battle { get; set; }
    }
}
