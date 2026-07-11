using ChronicleKeeper.Core.Entities.Social.Education;

namespace ChronicleKeeper.Core.Entities.Professions
{
    /// <summary>Join: Profession ↔ TradeSchool (composite PK, not a LoreEntity).</summary>
    public class ProfessionTradeSchool
    {
        public int ProfessionId { get; set; }
        public virtual Profession? Profession { get; set; }

        public int TradeSchoolId { get; set; }
        public virtual TradeSchool? TradeSchool { get; set; }
    }
}
