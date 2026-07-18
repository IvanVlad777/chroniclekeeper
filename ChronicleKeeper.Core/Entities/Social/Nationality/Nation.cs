using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
//using ChronicleKeeper.Core.Entities.Geography;
//using ChronicleKeeper.Core.Entities.Social.Politics;
//using ChronicleKeeper.Core.Entities.Social.Religions;
//using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Entities.Social.Structure;

namespace ChronicleKeeper.Core.Entities.Social.Nationality
{
    public class Nation : LoreEntity
    {
        public int Population { get; set; }

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public ICollection<Character> Characters { get; set; } = new List<Character>();

        //public ICollection<PoliticalParty> PoliticalParties { get; set; } = new List<PoliticalParty>(); // TODO: Uncomment when PoliticalParty entity is revived
        //public ICollection<Religion> StateReligions { get; set; } = new List<Religion>(); // TODO: Uncomment when Religion entity is revived
        //public ICollection<Language> LanguagesSpoken { get; set; } = new List<Language>(); // TODO: Uncomment when Language entity is revived
        //public ICollection<Culture> Culture { get; set; } = new List<Culture>(); // TODO: Uncomment when Culture entity is revived

        public int? SocialHierarchyId { get; set; }
        public SocialHierarchy? SocialHierarchy { get; set; }

        //public ICollection<DiplomaticAgreement> DiplomaticAgreements { get; set; } = new List<DiplomaticAgreement>(); // TODO: Uncomment when DiplomaticAgreement entity is revived

        //public ICollection<City> Cities { get; set; } = new List<City>(); // TODO: Uncomment when City entity is revived
        //public ICollection<Country> Countries { get; set; } = new List<Country>(); // TODO: Uncomment when Country entity is revived
    }
}