using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Politics
{
    public class DiplomaticAgreement : ILoreEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public virtual History? History { get; set; }

        public string AgreementType { get; set; } = string.Empty; // e.g., Trade, Peace Treaty
        public DateTime SignedDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public int? DurationYears { get; set; } // If the agreement has a fixed length

        // ✅ More details about the treaty
        public string Terms { get; set; } = string.Empty; // What each nation gains or loses

        // ✅ Power dynamics
        public bool IsUnequal { get; set; } // If one nation has significantly more benefits

        // ✅ Two main nations in the agreement
        //[ForeignKey("FirstNation")]
        public int FirstNationId { get; set; }

        //[ForeignKey("SecondNation")]
        public int SecondNationId { get; set; }

    }
}
