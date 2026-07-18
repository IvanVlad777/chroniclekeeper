using System.ComponentModel.DataAnnotations;

namespace ChronicleKeeper.Core.DTOs.Character
{
    /// <summary>Owned value type on Character — background / origin story.</summary>
    public class BackgroundInfoDto
    {
        [StringLength(200)]
        public string FamilyStatus { get; set; } = string.Empty;   // Orphan, Noble, Middle Class, etc.
        [StringLength(4000)]
        public string Childhood { get; set; } = string.Empty;      // Upbringing, experiences
        [StringLength(4000)]
        public string Upbringing { get; set; } = string.Empty;     // Education, lifestyle
        public bool IsImmigrant { get; set; }
        [StringLength(4000)]
        public string MigrationHistory { get; set; } = string.Empty;
    }

    /// <summary>Owned value type on Character — personality profile.</summary>
    public class PersonalityInfoDto
    {
        [StringLength(2000)]
        public string PersonalityTraits { get; set; } = string.Empty;
        [StringLength(2000)]
        public string Motivations { get; set; } = string.Empty;
        [StringLength(2000)]
        public string Virtues { get; set; } = string.Empty;
        [StringLength(2000)]
        public string Flaws { get; set; } = string.Empty;
        [StringLength(4000)]
        public string PsychologicalProfile { get; set; } = string.Empty;
        [StringLength(2000)]
        public string Fears { get; set; } = string.Empty;
        [StringLength(2000)]
        public string Ambitions { get; set; } = string.Empty;
    }
}
