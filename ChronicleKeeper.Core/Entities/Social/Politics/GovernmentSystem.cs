using ChronicleKeeper.Core.Entities.Base;
using static ChronicleKeeper.Core.Enums.SocietyEnums;

namespace ChronicleKeeper.Core.Entities.Social.Politics
{
    public class GovernmentSystem : LoreEntity
    {
        // Basic characteristics
        public bool IsDemocratic { get; set; } // True = Democratic, False = Authoritarian
        public bool IsMonarchic { get; set; } // True = Monarchy
        public bool IsReligious { get; set; } // True = Theocracy

        // Centralization
        public bool IsFederal { get; set; } // Has autonomous regions
        public bool IsCentralized { get; set; } // Power is centralized

        public int? PoliticalIdeologyId { get; set; }
        public PoliticalIdeology? PoliticalIdeology { get; set; }

        // Supports multiparty or one-party states
        public ICollection<PoliticalParty> PoliticalParties { get; set; } = new List<PoliticalParty>();

        public ElectionSystem ElectionSystem { get; set; }

        public ScaleLevel StabilityLevel { get; set; }
        public bool HasTermLimits { get; set; }
        public int? MaxTermLength { get; set; } // Max term length, if limited

        //public ICollection<Country> Countries { get; set; } = new List<Country>(); // TODO: Uncomment when Country entity is revived
        //public ICollection<City> Cities { get; set; } = new List<City>(); // TODO: Uncomment when City entity is revived
    }
}
