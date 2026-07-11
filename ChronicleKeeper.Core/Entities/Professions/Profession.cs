using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Entities.Social.Structure;

namespace ChronicleKeeper.Core.Entities.Professions
{
    public class Profession : LoreEntity
    {
        public string RequiredSkills { get; set; } = string.Empty; // List of skills needed
        public string WorkEnvironment { get; set; } = string.Empty; // E.g., "Outdoor", "Office", "Workshop"

        public virtual ICollection<Character> Characters { get; set; } = new List<Character>();
        public virtual ICollection<JobRank> JobRanks { get; set; } = new List<JobRank>();
        public virtual ICollection<Apprenticeship> Apprenticeships { get; set; } = new List<Apprenticeship>();
        public virtual ICollection<Specialisation> Specialisations { get; set; } = new List<Specialisation>();

        public virtual ICollection<ProfessionTradeSchool> TradeSchools { get; set; } = new List<ProfessionTradeSchool>();
        public virtual ICollection<ProfessionSapientSpecies> PracticedBySpecies { get; set; } = new List<ProfessionSapientSpecies>();
        public virtual ICollection<ProfessionSocialClass> SocialClasses { get; set; } = new List<ProfessionSocialClass>();

        //public virtual ICollection<Guild> Guilds { get; set; } = new List<Guild>(); // TODO: Uncomment when Guild entity is revived
        //public virtual ICollection<Corporation> Corporations { get; set; } = new List<Corporation>(); // TODO: Uncomment when Corporation entity is revived
        //public virtual ICollection<CorporateLeadership> CorporateLeaderships { get; set; } = new List<CorporateLeadership>(); // TODO: Uncomment when CorporateLeadership entity is revived
    }
}
