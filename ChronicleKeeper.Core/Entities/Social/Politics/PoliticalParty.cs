using ChronicleKeeper.Core.Entities.Base;
using static ChronicleKeeper.Core.Enums.SocietyEnums;

namespace ChronicleKeeper.Core.Entities.Social.Politics
{
    public class PoliticalParty : LoreEntity
    {
        public string IdeologyDescription { get; set; } = string.Empty;

        public int PoliticalIdeologyId { get; set; }
        public PoliticalIdeology PoliticalIdeology { get; set; } = null!;

        public int? GovernmentSystemId { get; set; }
        public GovernmentSystem? GovernmentSystem { get; set; }

        public ScaleLevel InfluenceLevel { get; set; }
        public bool IsBanned { get; set; } // Party is illegal

        //public ICollection<Country> Countries { get; set; } = new List<Country>(); // TODO: Uncomment when Country entity is revived
        //public ICollection<City> Cities { get; set; } = new List<City>(); // TODO: Uncomment when City entity is revived

        // TODO: Many-to-many cross-links to already-mapped entities (Faction, Nation) —
        // deferred to a dedicated pass so the join-table shape is decided once, holistically.
        //public ICollection<Faction> Factions { get; set; } = new List<Faction>();
        //public ICollection<Nation> Nations { get; set; } = new List<Nation>();
    }
}
