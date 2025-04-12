using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Economy;
using ChronicleKeeper.Core.Entities.Social.Structure;
using ChronicleKeeper.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Professions
{
    public class Profession : ILoreEntity
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
        public string RequiredSkills { get; set; } = string.Empty; // List of skills needed
        public string WorkEnvironment { get; set; } = string.Empty; // E.g., "Outdoor", "Office", "Workshop"

        public ICollection<Character> Character { get; set; } = new List<Character>();
        public ICollection<JobRank> JobRanks { get; set; } = new List<JobRank>(); // ✅ Ranks within a profession
        public ICollection<Apprenticeship> Apprenticeships { get; set; } = new List<Apprenticeship>(); // ✅ Training opportunities
        public ICollection<Specialisation> Specialisations { get; set; } = new List<Specialisation>();
        public ICollection<TradeSchool> TradeSchools { get; set; } = new List<TradeSchool>(); // ✅ Schools teaching this profession


        public ICollection<Guild> Guilds { get; set; } = new List<Guild>(); // ✅ Guilds that regulate this profession
        public ICollection<Corporation> Corporations { get; set; } = new List<Corporation>(); // ✅ Corporations that hire this profession
        public ICollection<CorporateLeadership> CorporateLeaderships { get; set; } = new List<CorporateLeadership>();
        public ICollection<SapientSpecies> PracticedBySpecies { get; set; } = new List<SapientSpecies>(); // ✅ Who works in this profession
        public ICollection<SocialClass> SocialClasses { get; set; } = new List<SocialClass>();
    }

}
