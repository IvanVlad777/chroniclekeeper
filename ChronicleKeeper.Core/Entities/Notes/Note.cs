using ChronicleKeeper.Core.Entities.Base;

namespace ChronicleKeeper.Core.Entities.Notes
{
    /// <summary>
    /// Freeform writer's note inside a world. Name (from LoreEntity) is the title.
    /// </summary>
    public class Note : LoreEntity
    {
        public string Content { get; set; } = string.Empty; // nvarchar(max)
    }
}
