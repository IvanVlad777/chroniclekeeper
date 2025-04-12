using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Economy;
using ChronicleKeeper.Core.Entities.Social.Structure;
using ChronicleKeeper.Core.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Social.Education
{
    public class EducationSystem : ILoreEntity
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

        public bool IsStateControlled { get; set; } // True = Government controls education
        public bool AllowsPrivateInstitutions { get; set; } // If private schools exist
        public bool IncludesReligiousEducation { get; set; } // If theology is part of learning
        public bool SupportsGuildTraining { get; set; } // If professions train people

        public ICollection<School> Schools { get; set; } = new List<School>();
        public ICollection<University> Universities { get; set; } = new List<University>();
        public ICollection<Guild> GuildsProvidingEducation { get; set; } = new List<Guild>(); // Apprenticeships

        public ICollection<Country> CountriesUsing { get; set; } = new List<Country>();
        public ICollection<City> CitiesUsing { get; set; } = new List<City>();
    }
}
