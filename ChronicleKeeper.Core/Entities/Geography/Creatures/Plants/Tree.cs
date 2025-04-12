using ChronicleKeeper.Core.Enums;

namespace ChronicleKeeper.Core.Entities.Geography.Creatures.Plants
{
    public class Tree : Plant
    {
        public double MaxHeight { get; set; } // Maximum tree height (meters)
        public int Lifespan { get; set; } // Average lifespan (years)
        public LeafType LeafType { get; set; } // Deciduous, Evergreen, etc.
    }
}
