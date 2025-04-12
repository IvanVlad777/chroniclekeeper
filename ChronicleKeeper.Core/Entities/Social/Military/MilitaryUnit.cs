using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ChronicleKeeper.Core.Entities.Social.Military
{
    public class MilitaryUnit : ILoreEntity
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

        public string UnitType { get; set; } = string.Empty; // e.g., Infantry, Cavalry, Artillery, Naval Fleet
        public int Size { get; set; }
        public bool IsElite { get; set; }

        //[ForeignKey("BelongsToArmy")]
        public int BelongsToArmyId { get; set; }
        public Army BelongsToArmy { get; set; } = null!;

        public ICollection<MilitaryRank> Ranks { get; set; } = new List<MilitaryRank>();
        public ICollection<MilitaryEquipment> Equipment { get; set; } = new List<MilitaryEquipment>();
    }
}
