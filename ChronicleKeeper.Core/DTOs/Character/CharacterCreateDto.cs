using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronicleKeeper.Core.DTOs.Character
{
    public class CharacterCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public bool IsArtificial { get; set; }

        public int SapientSpeciesId { get; set; }
        public int? NationId { get; set; }
        public int? ReligionId { get; set; }
        public int? ProfessionId { get; set; }
        public int? SocialClassId { get; set; }
        public int? FatherId { get; set; }
        public int? MotherId { get; set; }
    }
}
