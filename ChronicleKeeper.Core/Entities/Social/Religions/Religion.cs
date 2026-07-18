using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Characters;
//using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
//using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.Social.Cultures;

namespace ChronicleKeeper.Core.Entities.Social.Religions
{
    public class Religion : LoreEntity
    {
        public string CoreBeliefs { get; set; } = string.Empty; // Summary of religious doctrine
        public string Practices { get; set; } = string.Empty; // Rituals, ceremonies, festivals
        public bool HasDeities { get; set; } // Whether the religion is theistic or non-theistic
        public bool IsStateReligion { get; set; } // If officially recognized by a government

        public ICollection<Character> Followers { get; set; } = new List<Character>();

        //public ICollection<Deity> Deities { get; set; } = new List<Deity>(); // TODO: Uncomment when Deity entity is revived
        //public ICollection<ReligiousOrder> ReligiousOrders { get; set; } = new List<ReligiousOrder>(); // TODO: Uncomment when ReligiousOrder entity is revived
        //public ICollection<HolySite> HolySites { get; set; } = new List<HolySite>(); // TODO: Uncomment when HolySite entity is revived
        //public ICollection<ReligiousText> ReligiousTexts { get; set; } = new List<ReligiousText>(); // TODO: Uncomment when ReligiousText entity is revived
        public ICollection<Myth> Myths { get; set; } = new List<Myth>();
        public ICollection<Tradition> Traditions { get; set; } = new List<Tradition>();
        //public ICollection<Culture> Cultures { get; set; } = new List<Culture>(); // TODO: Uncomment when Culture entity is revived

        //public ICollection<Country> InCountries { get; set; } = new List<Country>(); // TODO: Uncomment when Country entity is revived
        //public ICollection<City> InCities { get; set; } = new List<City>(); // TODO: Uncomment when City entity is revived
    }
}
