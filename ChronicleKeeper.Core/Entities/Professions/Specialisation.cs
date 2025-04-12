using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Professions
{
    public class Specialisation : ILoreEntity
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
        public string Field { get; set; } = string.Empty; // e.g., Swordsmanship, Alchemy, Engineering

        //[ForeignKey("Profession")]
        public int ProfessionId { get; set; }
        public Profession? Profession { get; set; }


        public ICollection<Character> Experts { get; set; } = new List<Character>(); // ✅ Who specializes in this?
    }

}
