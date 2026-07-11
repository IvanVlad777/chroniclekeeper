using AutoMapper;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.DTOs.Character;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.EducationRecord;

public class CharacterProfile : Profile
{
    public CharacterProfile()
    {
        CreateMap<Character, CharacterDto>();
        CreateMap<CharacterDto, Character>();
        CreateMap<CharacterCreateDto, Character>();
        CreateMap<CharacterUpdateDto, Character>();

        CreateMap<Character, CharacterDetailsDto>()
            .ForMember(dest => dest.Father, opt => opt.MapFrom(src => src.Father == null ? null : new ReferenceDto { Id = src.Father.Id, Name = src.Father.Name }))
            .ForMember(dest => dest.Mother, opt => opt.MapFrom(src => src.Mother == null ? null : new ReferenceDto { Id = src.Mother.Id, Name = src.Mother.Name }))
            .ForMember(dest => dest.Species, opt => opt.MapFrom(src => src.SapientSpecies == null ? null : new ReferenceDto { Id = src.SapientSpecies.Id, Name = src.SapientSpecies.Name }))
            .ForMember(dest => dest.Race, opt => opt.MapFrom(src => src.Race == null ? null : new ReferenceDto { Id = src.Race.Id, Name = src.Race.Name }))
            .ForMember(dest => dest.SocialClass, opt => opt.MapFrom(src => src.SocialClass == null ? null : new ReferenceDto { Id = src.SocialClass.Id, Name = src.SocialClass.Name }))
            .ForMember(dest => dest.Nation, opt => opt.MapFrom(src => src.Nation == null ? null : new ReferenceDto { Id = src.Nation.Id, Name = src.Nation.Name }))
            .ForMember(dest => dest.Religion, opt => opt.MapFrom(src => src.Religion == null ? null : new ReferenceDto { Id = src.Religion.Id, Name = src.Religion.Name }))
            .ForMember(dest => dest.Profession, opt => opt.MapFrom(src => src.Profession == null ? null : new ReferenceDto { Id = src.Profession.Id, Name = src.Profession.Name }))
            .ForMember(dest => dest.Educations, opt => opt.MapFrom(src => src.Educations))
            .ForMember(dest => dest.ReligiousEducations, opt => opt.Ignore())
            .ForMember(dest => dest.Abilities, opt => opt.MapFrom(src => src.Abilities
                .Where(a => a.Ability != null)
                .Select(a => new ReferenceDto { Id = a.Ability!.Id, Name = a.Ability.Name })))
            .ForMember(dest => dest.Equipments, opt => opt.MapFrom(src => src.Equipments
                .Select(i => new ReferenceDto { Id = i.Id, Name = i.Name })))
            .ForMember(dest => dest.Factions, opt => opt.MapFrom(src => src.Memberships
                .Where(m => m.Faction != null)
                .Select(m => new ReferenceDto { Id = m.Faction!.Id, Name = m.Faction.Name })))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags
                .Where(t => t.Tag != null)
                .Select(t => new ReferenceDto { Id = t.Tag!.Id, Name = t.Tag.Name })))
            .ForMember(dest => dest.Relationships, opt => opt.MapFrom(src => src.Relationships
                .Select(r => new CharacterRelationshipDto
                {
                    Id = r.Id,
                    RelatedCharacterId = r.RelatedCharacterId,
                    RelatedCharacterName = r.RelatedCharacter == null ? string.Empty : r.RelatedCharacter.Name,
                    Type = r.Type.ToString(),
                    Description = r.Description,
                    IsSecret = r.IsSecret
                })));
    }
}
