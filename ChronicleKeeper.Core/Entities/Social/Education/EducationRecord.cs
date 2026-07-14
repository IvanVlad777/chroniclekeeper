using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Characters;

namespace ChronicleKeeper.Core.Entities.Social.Education
{
    public class EducationRecord : LoreEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Degree { get; set; } = string.Empty; // Bachelor's, Military Rank, Wizard Certification

        public int? CharacterId { get; set; }
        public virtual Character? Character { get; set; }

        public int? SchoolId { get; set; } // If it was a school
        public virtual School? School { get; set; }

        public int? UniversityId { get; set; } // If it was a university
        public virtual University? University { get; set; }

        public int? GuildId { get; set; }
        public virtual Economy.Guild? Guild { get; set; }
    }
}
