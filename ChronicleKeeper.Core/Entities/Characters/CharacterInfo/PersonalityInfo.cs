using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Core.Entities.Characters.CharacterInfo
{
    [Owned]
    public class PersonalityInfo
    {
        public string PersonalityTraits { get; set; } = string.Empty; // Reserved, Outgoing, etc.
        public string Motivations { get; set; } = string.Empty; // Ambitions and goals
        public string Virtues { get; set; } = string.Empty; // Positive characteristics
        public string Flaws { get; set; } = string.Empty; // Negative characteristics
        public string PsychologicalProfile { get; set; } = string.Empty; // Deep personality analysis
        public string Fears { get; set; } = string.Empty; // Phobias, anxieties
        public string Ambitions { get; set; } = string.Empty; // Personal aspirations
    }

}
