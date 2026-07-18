using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Social.Religions;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class Myth : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public string CreationStory { get; set; } = string.Empty; // How the world, gods, or creatures were created
        public string Symbolism { get; set; } = string.Empty; // Symbolic meaning behind the myth
        public bool HasReligiousConnections { get; set; } // If part of religious doctrine

        // Optional religion link — SetNull.
        public int? ReligionId { get; set; }
        public virtual Religion? Religion { get; set; }

        public int CultureId { get; set; }
        public virtual Culture? Culture { get; set; }

        // Optional deity link — SetNull. (Deity.MajorMyths is the reverse.)
        public int? DeityId { get; set; }
        public virtual Deity? Deity { get; set; }
    }
}
