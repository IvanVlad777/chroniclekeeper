using ChronicleKeeper.Core.Entities.Base;

namespace ChronicleKeeper.Core.Entities.Social.Education
{
    public class EducationSystem : LoreEntity
    {
        public bool IsStateControlled { get; set; } // True = Government controls education
        public bool AllowsPrivateInstitutions { get; set; } // If private schools exist
        public bool IncludesReligiousEducation { get; set; } // If theology is part of learning
        public bool SupportsGuildTraining { get; set; } // If professions train people

        public virtual ICollection<School> Schools { get; set; } = new List<School>();
        public virtual ICollection<University> Universities { get; set; } = new List<University>();

        //public virtual ICollection<Guild> GuildsProvidingEducation { get; set; } = new List<Guild>(); // TODO: Uncomment when Guild entity is revived
        //public virtual ICollection<Geography.Country> CountriesUsing { get; set; } = new List<Geography.Country>(); // TODO: Uncomment when Country entity is revived
        //public virtual ICollection<Geography.City> CitiesUsing { get; set; } = new List<Geography.City>(); // TODO: Uncomment when City entity is revived
    }
}
