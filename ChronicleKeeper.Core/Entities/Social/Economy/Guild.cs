using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Social.Politics;
using ChronicleKeeper.Core.Entities.Social.Economy;
using ChronicleKeeper.Core.Entities.Professions;
using System.Collections.Generic;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.Social.Education;
using System.ComponentModel.DataAnnotations.Schema;
using ChronicleKeeper.Core.Interfaces;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using System.ComponentModel.DataAnnotations;
using ChronicleKeeper.Core.Entities.Social.Structure;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class Guild : ILoreEntity
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

        public string GuildType { get; set; } = string.Empty; // Merchant, Artisan, Military, Thieves
        public string PrimaryActivity { get; set; } = string.Empty; // Blacksmithing, Magic, Trade, Smuggling
        public bool IsGovernmentSanctioned { get; set; } // If legally recognized

        //[ForeignKey("TaxationSystem")]
        public int? TaxationSystemId { get; set; } // Guilds are often taxed differently
        public TaxationSystem? TaxationSystem { get; set; }

        //[ForeignKey("Industry")]
        public int? IndustryId { get; set; } // The industry this guild belongs to
        public Industry? Industry { get; set; }

        //[ForeignKey("LegalSystem")]
        public int? LegalSystemId { get; set; } // If guilds have their own legal framework
        public LegalSystem? LegalSystem { get; set; }

        //[ForeignKey("EducationSystem")]
        public int? EducationSystemId { get; set; } 
        public EducationSystem? EducationSystem { get; set; }

        public ICollection<Country> PresentInCountries { get; set; } = new List<Country>();
        public ICollection<City> PresentInCities { get; set; } = new List<City>();
        public ICollection<Faction> Factions { get; set; } = new List<Faction>();


        public ICollection<Profession> MemberProfessions { get; set; } = new List<Profession>(); // Jobs in the guild

        public ICollection<GuildRank> GuildRanks { get; set; } = new List<GuildRank>(); // Hierarchy
        public ICollection<Apprenticeship> Apprenticeships { get; set; } = new List<Apprenticeship>();
        public ICollection<EducationRecord> Alumni { get; set; } = new List<EducationRecord>();
        public ICollection<SocialClass> SocialClasses { get; set; } = new List<SocialClass>();


    }
}
