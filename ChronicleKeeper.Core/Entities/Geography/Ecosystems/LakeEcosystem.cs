namespace ChronicleKeeper.Core.Entities.Geography.Ecosystems
{
    public class LakeEcosystem : WaterEcosystem
    {
        public double Volume { get; set; } // Water volume (km³)
        public double MaxDepth { get; set; } // Maximum depth (m)
        public bool IsFreshwater { get; set; } // True = Freshwater, False = Saltwater
    }
}
