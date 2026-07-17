using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Military
{
    public class MilitaryOrganization : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public string Type { get; set; } = string.Empty; // e.g., Army, Navy, Air Force
        public string Role { get; set; } = string.Empty; // e.g., Defensive, Offensive, Expeditionary

        // Optional doctrine — SetNull (Ivan's choice: an organization need not have a doctrine).
        public int? MilitaryDoctrineId { get; set; }
        public virtual MilitaryDoctrine? MilitaryDoctrine { get; set; }

        // Reverse of Army.MilitaryOrganizationId
        public virtual ICollection<Army> Armies { get; set; } = new List<Army>();

        // M:N (join entities)
        public virtual ICollection<MilitaryOrganizationCountry> Countries { get; set; } = new List<MilitaryOrganizationCountry>();
        public virtual ICollection<MilitaryOrganizationFaction> Factions { get; set; } = new List<MilitaryOrganizationFaction>();
    }
}
