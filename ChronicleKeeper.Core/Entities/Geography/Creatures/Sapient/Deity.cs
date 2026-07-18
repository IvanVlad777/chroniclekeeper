using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Entities.Social.Religions;
using static ChronicleKeeper.Core.Enums.CreatureEnums;

namespace ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient
{
    // Modelled as its own LoreEntity (NOT a SapientSpecies subtype) — the scaffold's
    // `Deity : SapientSpecies` polymorphism was unused, and inheriting a TPH root on the
    // already-populated SapientSpecies table would force a discriminator + backfill.
    public class Deity : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public string Domain { get; set; } = string.Empty; // e.g., War, Wisdom, Nature
        public string WorshipMethods { get; set; } = string.Empty; // How people worship this deity
        public bool IsMonotheistic { get; set; } // True = Sole god, False = Part of a pantheon

        // Optional religion — SetNull.
        public int? ReligionId { get; set; }
        public virtual Religion? Religion { get; set; }

        // Divine nature
        public DeityType DeityType { get; set; } // Enum: Elemental, Cosmic, Anthropomorphic, Alien
        public bool IsImmortal { get; set; }
        public bool CanManifestPhysically { get; set; }
        public bool GrantsPowers { get; set; }

        // 1:N reverse collections (FK lives on the child).
        public virtual ICollection<ReligiousText> SacredTexts { get; set; } = new List<ReligiousText>();
        public virtual ICollection<HolySite> SacredSitesOfDeity { get; set; } = new List<HolySite>();
        public virtual ICollection<Myth> MajorMyths { get; set; } = new List<Myth>();

        // M:N with ReligiousOrder (join entity) — owner side.
        public virtual ICollection<DeityReligiousOrder> OrdersDedicatedToDeity { get; set; } = new List<DeityReligiousOrder>();

        // Self-referencing M:N (join entities) — owner side.
        public virtual ICollection<DeityAlliance> AlliedDeities { get; set; } = new List<DeityAlliance>();
        public virtual ICollection<DeityRivalry> RivalDeities { get; set; } = new List<DeityRivalry>();
    }
}
