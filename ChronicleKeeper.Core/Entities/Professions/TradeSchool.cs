using ChronicleKeeper.Core.Entities.Social.Education;

namespace ChronicleKeeper.Core.Entities.Professions
{
    /// <summary>
    /// A TradeSchool IS a School (TPH: shared "Schools" table, "SchoolType" discriminator)
    /// with a few extra vocational fields.
    /// </summary>
    public class TradeSchool : School
    {
        public string Specialization { get; set; } = string.Empty; // Blacksmithing, Carpentry, Alchemy, etc.
        public int DurationYears { get; set; } // Average length of study
        public bool IsGovernmentRecognized { get; set; } // True if officially certified

        public virtual ICollection<ProfessionTradeSchool> TrainedProfessions { get; set; } = new List<ProfessionTradeSchool>();
        public virtual ICollection<Apprenticeship> Apprenticeships { get; set; } = new List<Apprenticeship>();
    }
}
