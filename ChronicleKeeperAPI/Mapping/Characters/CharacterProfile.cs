using AutoMapper;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.DTOs.Character;
using ChronicleKeeper.Core.DTOs;

public class CharacterProfile : Profile
{
    public CharacterProfile()
    {
        CreateMap<Character, CharacterDto>()
            .ForMember(dest => dest.Species, opt => opt.MapFrom(src => new ReferenceDto { Id = src.SapientSpecies.Id, Name = src.SapientSpecies.Name }))
            .ForMember(dest => dest.Religion, opt => opt.MapFrom(src => src.Religion == null ? null : new ReferenceDto { Id = src.Religion.Id, Name = src.Religion.Name }))
            .ForMember(dest => dest.Nation, opt => opt.MapFrom(src => src.Nation == null ? null : new ReferenceDto { Id = src.Nation.Id, Name = src.Nation.Name }))
            .ForMember(dest => dest.Profession, opt => opt.MapFrom(src => src.Profession == null ? null : new ReferenceDto { Id = src.Profession.Id, Name = src.Profession.Name }))
            .ForMember(dest => dest.SocialClass, opt => opt.MapFrom(src => src.SocialClass == null ? null : new ReferenceDto { Id = src.SocialClass.Id, Name = src.SocialClass.Name }))
            .ForMember(dest => dest.Father, opt => opt.MapFrom(src => src.Father == null ? null : new ReferenceDto { Id = src.Father.Id, Name = src.Father.Name }))
            .ForMember(dest => dest.Mother, opt => opt.MapFrom(src => src.Mother == null ? null : new ReferenceDto { Id = src.Mother.Id, Name = src.Mother.Name }));

        CreateMap<CharacterDto, Character>();

        CreateMap<CharacterCreateDto, Character>();
    }
}