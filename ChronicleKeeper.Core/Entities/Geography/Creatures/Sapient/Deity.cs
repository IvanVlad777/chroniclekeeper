using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Entities.Social.Religions;
using System.ComponentModel.DataAnnotations.Schema;
using static ChronicleKeeper.Core.Enums.CreatureEnums;

namespace ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient
{
    public class Deity : SapientSpecies
    {
        public string Domain { get; set; } = string.Empty; // e.g., War, Wisdom, Nature
        public string WorshipMethods { get; set; } = string.Empty; // How people worship this deity
        public bool IsMonotheistic { get; set; } // True = Sole god, False = Part of a pantheon

        //[ForeignKey("Religion")]
        public int? ReligionId { get; set; }
        public Religion? Religion { get; set; }

        // ✅ Božanska priroda
        public DeityType DeityType { get; set; } // Enum: Elemental, Cosmic, Anthropomorphic, Alien
        public bool IsImmortal { get; set; } // True ako ne može umrijeti
        public bool CanManifestPhysically { get; set; } // Može li hodati među smrtnicima?
        public bool GrantsPowers { get; set; } // Može li davati magične moći vjernicima?

        // ✅ Mitovi i sveti tekstovi
        public ICollection<ReligiousText> SacredTexts { get; set; } = new List<ReligiousText>(); // Sveti spisi vezani uz ovo božanstvo
        public ICollection<ReligiousOrder> OrdersDedicatedToDeity { get; set; } = new List<ReligiousOrder>(); // Redovi posvećeni ovom božanstvu
        public ICollection<HolySite> SacredSitesOfDeity { get; set; } = new List<HolySite>(); // Sveta mjesta povezana s ovim bogom
        public ICollection<Myth> MajorMyths { get; set; } = new List<Myth>(); // Sveta mjesta povezana s ovim bogom

        // ✅ Odnosi s drugim božanstvima
        public ICollection<Deity> AlliedDeities { get; set; } = new List<Deity>(); // Prijatelji
        public ICollection<Deity> RivalDeities { get; set; } = new List<Deity>(); // Protivnici
    }
}
