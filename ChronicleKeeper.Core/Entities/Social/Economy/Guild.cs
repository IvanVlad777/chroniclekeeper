using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Professions;
using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Entities.Social.Politics;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class Guild : LoreEntity
    {
        public string GuildType { get; set; } = string.Empty; // Merchant, Artisan, Military, Thieves
        public string PrimaryActivity { get; set; } = string.Empty; // Blacksmithing, Magic, Trade, Smuggling
        public bool IsGovernmentSanctioned { get; set; } // If legally recognized

        public int? TaxationSystemId { get; set; } // Guilds are often taxed differently
        public virtual TaxationSystem? TaxationSystem { get; set; }

        public int? IndustryId { get; set; } // The industry this guild belongs to
        public virtual Industry? Industry { get; set; }

        public int? LegalSystemId { get; set; } // If guilds have their own legal framework
        public virtual LegalSystem? LegalSystem { get; set; }

        public int? EducationSystemId { get; set; }
        public virtual EducationSystem? EducationSystem { get; set; }

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public virtual ICollection<GuildRank> GuildRanks { get; set; } = new List<GuildRank>(); // Hierarchy
        public virtual ICollection<Apprenticeship> Apprenticeships { get; set; } = new List<Apprenticeship>();
        public virtual ICollection<EducationRecord> Alumni { get; set; } = new List<EducationRecord>();

        public virtual ICollection<GuildFaction> Factions { get; set; } = new List<GuildFaction>();
        public virtual ICollection<GuildProfession> MemberProfessions { get; set; } = new List<GuildProfession>(); // Jobs in the guild
        public virtual ICollection<GuildSocialClass> SocialClasses { get; set; } = new List<GuildSocialClass>();

        //public ICollection<Country> PresentInCountries { get; set; } = new List<Country>(); // TODO: Deferred with the Country/City M:N bucket
        //public ICollection<City> PresentInCities { get; set; } = new List<City>(); // TODO: Deferred with the Country/City M:N bucket
    }
}
