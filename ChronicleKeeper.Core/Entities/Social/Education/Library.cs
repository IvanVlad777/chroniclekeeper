using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Geography;

namespace ChronicleKeeper.Core.Entities.Social.Education
{
    public class Library : LoreEntity
    {
        public bool IsPublic { get; set; } // Open to all citizens
        public bool FocusesOnMagic { get; set; } // Specializes in magical texts
        public bool FocusesOnHistory { get; set; } // Archives and historical records

        public virtual ICollection<LibraryScholar> Scholars { get; set; } = new List<LibraryScholar>();

        public int? UniversityId { get; set; }
        public virtual University? University { get; set; }

        public int? LocationId { get; set; }
        public virtual Location? Location { get; set; }
    }
}
