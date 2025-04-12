using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronicleKeeper.Core.Enums
{
    public enum PlantType
    {
        Tree = 1,
        Shrub = 2,
        Herb = 3,
        Grass = 4,
        Vine = 5
    }

    public enum LeafType
    {
        Deciduous = 1, // Sheds leaves seasonally
        Evergreen = 2  // Keeps leaves year-round
    }

    public enum CropType
    {
        Grain = 1,
        Fruit = 2,
        Vegetable = 3,
        Legume = 4,
        Herb = 5
    }

    public enum SunlightRequirement
    {
        FullSun = 1,
        PartialSun = 2,
        Shade = 3
    }

    public enum SoilType
    {
        Sandy = 1,
        Clay = 2,
        Loamy = 3,
        Rocky = 4
    }

    public enum TemperatureRange
    {
        Cold = 1, // Arctic, tundra
        Temperate = 2, // Forests, plains
        Hot = 3 // Desert, rainforest
    }
}
