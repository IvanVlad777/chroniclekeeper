using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronicleKeeper.Core.DTOs.Character
{
    namespace ChronicleKeeper.API.Dtos
    {
        public class CharacterDetailsDto : CharacterDto
        {
            public List<ReferenceDto> Abilities { get; set; } = new();
            public List<ReferenceDto> Hobbies { get; set; } = new();
            public List<ReferenceDto> Equipments { get; set; } = new();
            public List<ReferenceDto> Clothing { get; set; } = new();
            public List<ReferenceDto> Educations { get; set; } = new();
            public List<ReferenceDto> Specialisations { get; set; } = new();
            public List<ReferenceDto> Factions { get; set; } = new();
            public List<ReferenceDto> Siblings { get; set; } = new();
        }
    }

}
