using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.Tags;
//using ChronicleKeeper.Core.Entities.Characters.Equipment;
//using ChronicleKeeper.Core.Entities.HistoryTimelines;
//using ChronicleKeeper.Core.Entities.Social.Economy;
//using ChronicleKeeper.Core.Entities.Social.Military;
//using ChronicleKeeper.Core.Entities.Social.Politics;
//using ChronicleKeeper.Core.Entities.Social.Religions;
using static ChronicleKeeper.Core.Enums.SocietyEnums;

namespace ChronicleKeeper.Core.Entities.Social
{
    public class Faction : LoreEntity
    {
        public FactionType Type { get; set; } // Political, Religious, Military, Economic
        public bool IsSecretive { get; set; } // If their existence is hidden
        public string Motto { get; set; } = string.Empty;

        public int? LeaderId { get; set; }
        public virtual Character? Leader { get; set; }

        public int? HeadquartersId { get; set; }
        public virtual Location? Headquarters { get; set; }

        public virtual ICollection<FactionMember> Members { get; set; } = new List<FactionMember>();
        public virtual ICollection<FactionTag> Tags { get; set; } = new List<FactionTag>();

        //public ICollection<Country> OperatesInCountries { get; set; } = new List<Country>(); // TODO: Uncomment when Country entity is revived
        //public ICollection<City> OperatesInCities { get; set; } = new List<City>(); // TODO: Uncomment when City entity is revived
        //public ICollection<Guild> ConnectedGuilds { get; set; } = new List<Guild>(); // TODO: Uncomment when Guild entity is revived
        //public ICollection<PoliticalParty> PoliticalConnections { get; set; } = new List<PoliticalParty>(); // TODO: Uncomment when PoliticalParty entity is revived
        //public ICollection<ReligiousOrder> ReligiousConnections { get; set; } = new List<ReligiousOrder>(); // TODO: Uncomment when ReligiousOrder entity is revived
        //public ICollection<Corporation> FinancialBackers { get; set; } = new List<Corporation>(); // TODO: Uncomment when Corporation entity is revived
        //public ICollection<Item> NotableItemsInPossesion { get; set; } = new List<Item>(); // TODO: Uncomment when Item entity is revived
        //public ICollection<MilitaryOrganization> MilitaryConnections { get; set; } = new List<MilitaryOrganization>(); // TODO: Uncomment when MilitaryOrganization entity is revived
        //public int? ArmyId { get; set; } // TODO: Uncomment when Army entity is revived
        //public Army? Army { get; set; }
    }
}
