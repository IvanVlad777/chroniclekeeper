using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Religions;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class Tradition : ILoreEntity
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

        public string Practice { get; set; } = string.Empty; // Wedding ceremonies, rites of passage
        public bool IsReligious { get; set; } // If tied to a religion

        //[ForeignKey("Religion")]
        public int? ReligionId { get; set; }
        public Religion? Religion { get; set; }


        //[ForeignKey("Culture")]
        public int CultureId { get; set; }
        public Culture? Culture { get; set; }
    }
}
