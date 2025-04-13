using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronicleKeeper.Core.DTOs.Character
{
    public class CharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public string HairColor { get; set; } = string.Empty;
        public string EyeColor { get; set; } = string.Empty;
        public string SpecialPhysicalFeatures { get; set; } = string.Empty;
        public bool IsArtificial { get; set; }

        public ReferenceDto? Species { get; set; }
        public ReferenceDto? Religion { get; set; }
        public ReferenceDto? Nation { get; set; }
        public ReferenceDto? Profession { get; set; }
        public ReferenceDto? SocialClass { get; set; }
        public ReferenceDto? Father { get; set; }
        public ReferenceDto? Mother { get; set; }
    }
}