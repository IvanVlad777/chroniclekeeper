using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.Social.Religions;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Social.Education
{
    public class ReligiousEducation : LoreEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public bool Ordained { get; set; }

        public int? CharacterId { get; set; }
        public virtual Character? Character { get; set; }

        // Optional order that runs this training — SetNull. (ReligiousOrder.ClergyTraining is the reverse.)
        public int? ReligiousOrderId { get; set; }
        public virtual ReligiousOrder? ReligiousOrder { get; set; }

        [Required]
        public int ReligionId { get; set; }
        public virtual Religion Religion { get; set; } = null!;
    }
}
