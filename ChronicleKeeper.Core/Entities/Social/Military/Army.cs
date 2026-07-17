using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

namespace ChronicleKeeper.Core.Entities.Social.Military
{
    public class Army : LoreEntity
    {
        public int? HistoryId { get; set; }
        public virtual History? History { get; set; }

        public bool IsStandingArmy { get; set; } // Permanent army or wartime force?
        public int Size { get; set; } // Total soldiers

        // Optional pointers — SetNull.
        public int? CityId { get; set; }
        public virtual City? City { get; set; }

        public int? MilitaryOrganizationId { get; set; }
        public virtual MilitaryOrganization? MilitaryOrganization { get; set; }

        public int? FactionId { get; set; }
        public virtual Faction? Faction { get; set; }

        public virtual ICollection<MilitaryUnit> Units { get; set; } = new List<MilitaryUnit>();

        // M:N with Battle (join entity)
        public virtual ICollection<ArmyBattle> Battles { get; set; } = new List<ArmyBattle>();
    }
}
