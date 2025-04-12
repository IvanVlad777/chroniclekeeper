using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronicleKeeper.Core.Enums
{
    public class ClimateEnums
    {
        public enum ClimateZoneType
        {
            Tropical = 1,
            Arid = 2,
            Temperate = 3,
            Continental = 4,
            Polar = 5
        }

        public enum WindDirection
        {
            North = 1,
            South = 2,
            East = 3,
            West = 4,
            Northeast = 5,
            Northwest = 6,
            Southeast = 7,
            Southwest = 8,
            Variable = 9
        }

        public enum NotableWeatherPhenomena
        {
            None = 0,
            Tornado = 1,
            Hurricane = 2,
            Monsoon = 3,
            Sandstorm = 4,
            Blizzard = 5,
            Thunderstorm = 6,
            Drought = 7,
            Fog = 8
        }

        public enum WeatherPatternType
        {
            Normal = 1,
            Monsoon = 2,
            DesertStorm = 3,
            HurricaneSeason = 4,
            ArcticBlast = 5,
            HeatWave = 6
        }

        public enum Frequency
        {
            Rare = 1,
            Seasonal = 2,
            Frequent = 3,
            Constant = 4
        }

        public enum WeatherEffect
        {
            None = 0,
            Flooding = 1,
            Drought = 2,
            HighWinds = 3,
            ExtremeCold = 4,
            ExtremeHeat = 5
        }
    }
}
