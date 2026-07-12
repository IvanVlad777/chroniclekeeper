namespace ChronicleKeeper.Core.Entities.Geography.Ecosystems
{
    /// <summary>
    /// Which kind of water body (Lake/Sea/Ocean/River) is now carried by Location.Type + the
    /// CLR subtype itself — the dormant scaffold's own "new WaterBodyType Type" shadow would have
    /// duplicated that and hidden the base Location.Type used for generic type filtering, so it's
    /// dropped rather than revived (WaterBodyType enum stays defined, just unused).
    /// </summary>
    public abstract class WaterEcosystem : Ecosystem
    {
        // Named WaterDepth, not Depth, to avoid a TPH column-naming collision with the unrelated
        // CaveEcosystem.Depth (own, different sibling branch) — same class of gotcha as the
        // Plant/Fungus.ScientificName sibling collision from the Creature round, but this one
        // fragmented the shared inherited column across Lake/Sea/Ocean/River when left unrenamed.
        public double WaterDepth { get; set; } // Average depth (m)
    }
}
