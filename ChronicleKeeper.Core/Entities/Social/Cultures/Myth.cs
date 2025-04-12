using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Religions;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class Myth : ILoreEntity
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

        public string CreationStory { get; set; } = string.Empty; // How the world, gods, or creatures were created
        public string Symbolism { get; set; } = string.Empty; // Symbolic meaning behind the myth
        public bool HasReligiousConnections { get; set; } // If part of religious doctrine

        //[ForeignKey("Religion")]
        public int? ReligionId { get; set; }
        public Religion? Religion { get; set; }


        //[ForeignKey("Culture")]
        public int CultureId { get; set; }
        public Culture? Culture { get; set; }


        //[ForeignKey("Deity")]
        public int DeityId { get; set; }
        public Deity? Deity { get; set; }
    }

}
