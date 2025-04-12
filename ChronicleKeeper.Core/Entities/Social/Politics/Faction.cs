using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Social.Politics;
using ChronicleKeeper.Core.Entities.Social.Religions;
using ChronicleKeeper.Core.Entities.Social.Economy;
using ChronicleKeeper.Core.Entities.Social.Military;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.Geography;
using static ChronicleKeeper.Core.Enums.SocietyEnums;
using ChronicleKeeper.Core.Entities.Characters.Equipment;
using System.ComponentModel.DataAnnotations.Schema;
using ChronicleKeeper.Core.Interfaces;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.Entities.Social
{
    public class Faction : ILoreEntity
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

        public FactionType Type { get; set; } // Political, Religious, Military, Economic
        public bool IsSecretive { get; set; } // If their existence is hidden

        public ICollection<Character> Members { get; set; } = new List<Character>(); // Characters belonging to this faction
        public ICollection<Country> OperatesInCountries { get; set; } = new List<Country>(); // Which countries allow or oppose them
        public ICollection<City> OperatesInCities { get; set; } = new List<City>(); // Which cities allow or oppose them
        public ICollection<Guild> ConnectedGuilds { get; set; } = new List<Guild>(); // If tied to a profession
        public ICollection<PoliticalParty> PoliticalConnections { get; set; } = new List<PoliticalParty>(); // If they influence governments
        public ICollection<ReligiousOrder> ReligiousConnections { get; set; } = new List<ReligiousOrder>(); // If tied to a faith
        public ICollection<Corporation> FinancialBackers { get; set; } = new List<Corporation>(); // If backed by economic groups
        public ICollection<Item> NotableItemsInPossesion { get; set; } = new List<Item>(); // If tied to military power

        public ICollection<MilitaryOrganization> MilitaryConnections { get; set; } = new List<MilitaryOrganization>(); // If tied to military power

        //[ForeignKey("Army")]
        //public int? ArmyId { get; set; }
        public Army? Army { get; set; }
    }
}
