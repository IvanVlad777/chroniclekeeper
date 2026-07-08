using ChronicleKeeper.Core.Entities.Base;

namespace ChronicleKeeper.Core.Entities.Tags
{
    /// <summary>
    /// Writer-defined label, unique per world (e.g. "protagonist", "act-2", "needs-revision").
    /// Attached to entities through per-type join tables (CharacterTag, LocationTag, FactionTag).
    /// </summary>
    public class Tag : LoreEntity
    {
    }
}
