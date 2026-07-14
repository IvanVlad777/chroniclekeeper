using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Economy
{
    public class ExtractionMethod : LoreEntity
    {
        public string MethodType { get; set; } = string.Empty; // Mining, Farming, Drilling, Alchemy
        public bool IsSustainable { get; set; } // True if eco-friendly

        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public virtual ICollection<NaturalResource> ResourcesExtracted { get; set; } = new List<NaturalResource>();
    }
}
