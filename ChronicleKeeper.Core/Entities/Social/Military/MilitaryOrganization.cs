using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Social.Military
{
    public class MilitaryOrganization : ILoreEntity
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

        public string Type { get; set; } = string.Empty; // e.g., Army, Navy, Air Force
        public string Role { get; set; } = string.Empty; // e.g., Defensive, Offensive, Expeditionary

        public ICollection<Country> Countries { get; set; } = new List<Country>();
        public ICollection<Army> Armies { get; set; } = new List<Army>();
        public ICollection<Faction> Factions { get; set; } = new List<Faction>();


        //[ForeignKey("MilitaryDoctrine")]
        public int MilitaryDoctrineId { get; set; }
        public MilitaryDoctrine MilitaryDoctrine { get; set; } = null!;

    }
}
