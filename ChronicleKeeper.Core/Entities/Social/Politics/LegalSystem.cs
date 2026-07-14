using ChronicleKeeper.Core.Entities.Base;
using static ChronicleKeeper.Core.Enums.SocietyEnums;

namespace ChronicleKeeper.Core.Entities.Social.Politics
{
    public class LegalSystem : LoreEntity
    {
        public string Laws { get; set; } = string.Empty; // Overview of legal structure

        public ScaleLevel JudicialIndependence { get; set; }
        public PunishmentMethods PunishmentMethods { get; set; } // e.g. Death Penalty, Forced Labor, Fines

        //public ICollection<City> Cities { get; set; } = new List<City>(); // TODO: Uncomment when City entity is revived
        //public ICollection<Country> Countries { get; set; } = new List<Country>(); // TODO: Uncomment when Country entity is revived
        public ICollection<Economy.Guild> Guilds { get; set; } = new List<Economy.Guild>(); // Reverse nav of Guild.LegalSystemId
    }
}
