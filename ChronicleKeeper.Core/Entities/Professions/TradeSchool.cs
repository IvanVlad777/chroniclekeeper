using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Social.Education;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronicleKeeper.Core.Entities.Professions
{
    public class TradeSchool : School
    {
        public string Specialization { get; set; } = string.Empty; // Blacksmithing, Carpentry, Alchemy, etc.
        public int DurationYears { get; set; } // Average length of study
        public bool IsGovernmentRecognized { get; set; } // True if officially certified

        public ICollection<Profession> TrainedProfessions { get; set; } = new List<Profession>(); // ✅ Professions taught
        public ICollection<Apprenticeship> Apprenticeships { get; set; } = new List<Apprenticeship>(); // ✅ Training opportunities

    }

}
