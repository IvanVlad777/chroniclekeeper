using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Social.Nationality;

namespace ChronicleKeeper.Core.Entities.Social.Politics
{
    public class DiplomaticAgreement : LoreEntity
    {
        public string AgreementType { get; set; } = string.Empty; // e.g., Trade, Peace Treaty

        // Fictional dates as plain strings — see CLAUDE.md "Fictional dates" convention
        // (stopgap until a proper user-defined-calendar system is designed).
        public string SignedDate { get; set; } = string.Empty;
        public string? TerminationDate { get; set; }
        public int? DurationYears { get; set; } // If the agreement has a fixed length

        public string Terms { get; set; } = string.Empty; // What each nation gains or loses
        public bool IsUnequal { get; set; } // If one nation has significantly more benefits

        public int FirstNationId { get; set; }
        public Nation FirstNation { get; set; } = null!;

        public int SecondNationId { get; set; }
        public Nation SecondNation { get; set; } = null!;
    }
}
