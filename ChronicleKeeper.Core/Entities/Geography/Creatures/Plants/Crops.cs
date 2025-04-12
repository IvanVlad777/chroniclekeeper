using ChronicleKeeper.Core.Enums;

namespace ChronicleKeeper.Core.Entities.Geography.Creatures.Plants
{
    public class Crop : Plant
    {
        public double YieldPerHectare { get; set; } // Average production yield
        public CropType CropType { get; set; } // Grain, Fruit, Vegetable, etc.
        public bool IsDomesticated { get; set; } // True if commonly farmed
    }
}
