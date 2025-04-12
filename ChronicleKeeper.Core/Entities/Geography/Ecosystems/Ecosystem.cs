using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography.Climate;
using ChronicleKeeper.Core.Entities.Geography.Creatures;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Animals;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Fungi;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Plants;
using ChronicleKeeper.Core.Entities.Professions;
using ChronicleKeeper.Core.Entities.Social.Economy;
using ChronicleKeeper.Core.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronicleKeeper.Core.Entities.Geography.Ecosystems
{
    public abstract class Ecosystem : Location
    {
        public string UniqueFeatures { get; set; } = string.Empty;

        // ✅ Natural Resources
        public ICollection<Creature> NaturalHabitats { get; set; } = new List<Creature>();

        // ✅ Geographic Links
        //[ForeignKey("Region")]
        //public int RegionId { get; set; }
        //public Region? Region { get; set; }
    }
}
