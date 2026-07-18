using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Entities.Social.Politics;

namespace ChronicleKeeper.Core.Entities.Social.Religions
{
    public class ReligiousOrder : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public string OrderType { get; set; } = string.Empty; // Clergy, Monastic, Cult, Sect
        public string Beliefs { get; set; } = string.Empty; // Unique ideology of the order
        public bool IsMilitant { get; set; } // Whether the order is military-focused
        public bool IsSecretive { get; set; } // Whether it operates in secrecy

        // Optional religion — SetNull.
        public int? ReligionId { get; set; }
        public virtual Religion? Religion { get; set; }

        // 1:N reverse of ReligiousEducation.ReligiousOrderId.
        public virtual ICollection<ReligiousEducation> ClergyTraining { get; set; } = new List<ReligiousEducation>();

        // M:N with Deity (join entity) — owner side.
        public virtual ICollection<DeityReligiousOrder> Deities { get; set; } = new List<DeityReligiousOrder>();

        // M:N with Faction (join entity) — owner side; Faction is the asymmetric target.
        public virtual ICollection<ReligiousOrderFaction> Factions { get; set; } = new List<ReligiousOrderFaction>();

        //public ICollection<Country> OperatesInCountries { get; set; } = new List<Country>(); // TODO: Uncomment when Country gets a vertical (R5)
        //public ICollection<City> OperatesInCities { get; set; } = new List<City>(); // TODO: Uncomment when City gets a vertical (R5)
    }
}
