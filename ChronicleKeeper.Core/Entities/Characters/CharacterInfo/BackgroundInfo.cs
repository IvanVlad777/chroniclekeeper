using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Core.Entities.Characters.CharacterInfo
{
    [Owned]
    public class BackgroundInfo
    {
        public string FamilyStatus { get; set; } = string.Empty; // Orphan, Noble, Middle Class, etc.
        public string Childhood { get; set; } = string.Empty; // Upbringing, experiences
        public string Upbringing { get; set; } = string.Empty; // Education, lifestyle
        public bool IsImmigrant { get; set; } = false; // Whether they moved to a new nation
        public string MigrationHistory { get; set; } = string.Empty; // Details on migration
    }

}
