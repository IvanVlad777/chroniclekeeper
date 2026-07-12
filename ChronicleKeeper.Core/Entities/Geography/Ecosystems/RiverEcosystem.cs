namespace ChronicleKeeper.Core.Entities.Geography.Ecosystems
{
    /// <summary>
    /// SourceLocationId/MouthLocationId are point references (where the river starts/ends), not
    /// hierarchy — unlike Sea→Ocean/Mountain→MountainRange they don't ride on ParentLocationId.
    /// Both Restrict (see RiverEcosystemConfiguration): two SetNull FKs from one table to the same
    /// target table hit SQL Server's multiple-cascade-path error (same lesson as OwnershipHistory's
    /// PreviousOwner/NewOwner), so both are Restrict with explicit nulling before delete instead.
    /// </summary>
    public class RiverEcosystem : WaterEcosystem
    {
        public double RiverLength { get; set; } // in kilometers, named to avoid colliding with MountainRange.MountainRangeLength

        public int? SourceLocationId { get; set; }
        public Location? SourceLocation { get; set; }

        public int? MouthLocationId { get; set; }
        public Location? MouthLocation { get; set; }
    }
}
