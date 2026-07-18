using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Cultures
{
    public class Cuisine : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public string MainIngredients { get; set; } = string.Empty; // Common ingredients
        public string CookingMethods { get; set; } = string.Empty; // Techniques used
        public bool IsVegetarian { get; set; } // Whether it's primarily plant-based
        public string TypicalDishes { get; set; } = string.Empty;

        public int CultureId { get; set; }
        public virtual Culture? Culture { get; set; }
    }
}
