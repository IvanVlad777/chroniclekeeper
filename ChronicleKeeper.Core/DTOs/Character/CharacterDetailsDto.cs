namespace ChronicleKeeper.Core.DTOs.Character
{
    public class CharacterDetailsDto : CharacterDto
    {
        public ReferenceDto? Father { get; set; }
        public ReferenceDto? Mother { get; set; }
        public ReferenceDto? Species { get; set; }
        public ReferenceDto? Race { get; set; }
        public ReferenceDto? SocialClass { get; set; }
        public ReferenceDto? Nation { get; set; }
        public List<ReferenceDto> Factions { get; set; } = new();
        public List<ReferenceDto> Tags { get; set; } = new();
        public List<CharacterRelationshipDto> Relationships { get; set; } = new();

        // TODO: Otkomentirati kada budem dodavao veze
        //public List<ReferenceDto> Abilities { get; set; } = new();
        //public List<ReferenceDto> Hobbies { get; set; } = new();
        //public List<ReferenceDto> Equipments { get; set; } = new();
        //public List<ReferenceDto> Clothing { get; set; } = new();
        //public List<ReferenceDto> Educations { get; set; } = new();
        //public List<ReferenceDto> Specialisations { get; set; } = new();
        //public List<ReferenceDto> Siblings { get; set; } = new();
    }

    public class CharacterRelationshipDto
    {
        public int Id { get; set; }
        public int RelatedCharacterId { get; set; }
        public string RelatedCharacterName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsSecret { get; set; }
    }
}
