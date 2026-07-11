using ChronicleKeeper.Core.Entities.Base;

namespace ChronicleKeeper.Core.Entities.Professions
{
    public class Specialisation : LoreEntity
    {
        public string Field { get; set; } = string.Empty; // e.g., Swordsmanship, Alchemy, Engineering

        public int? ProfessionId { get; set; }
        public virtual Profession? Profession { get; set; }

        //public virtual ICollection<Character> Experts { get; set; } = new List<Character>(); // TODO: Uncomment when Character many-to-many cross-links are revived
    }
}
