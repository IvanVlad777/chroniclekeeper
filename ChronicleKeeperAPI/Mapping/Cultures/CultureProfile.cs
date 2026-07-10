using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.Culture;
using ChronicleKeeper.Core.Entities.Social.Cultures;

public class CultureProfile : Profile
{
    public CultureProfile()
    {
        CreateMap<Culture, CultureDto>();
        CreateMap<CultureCreateDto, Culture>();
        CreateMap<CultureUpdateDto, Culture>();

        CreateMap<Culture, CultureDetailsDto>()
            .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language == null ? null : new ReferenceDto { Id = src.Language.Id, Name = src.Language.Name }))
            .ForMember(dest => dest.Religion, opt => opt.MapFrom(src => src.Religion == null ? null : new ReferenceDto { Id = src.Religion.Id, Name = src.Religion.Name }))
            .ForMember(dest => dest.Nations, opt => opt.MapFrom(src => src.Nations
                .Select(cn => new ReferenceDto { Id = cn.Nation!.Id, Name = cn.Nation!.Name })))
            .ForMember(dest => dest.PracticedBySpecies, opt => opt.MapFrom(src => src.PracticedBySpecies
                .Select(cs => new ReferenceDto { Id = cs.SapientSpecies!.Id, Name = cs.SapientSpecies!.Name })))
            .ForMember(dest => dest.InfluencedSocialClasses, opt => opt.MapFrom(src => src.InfluencedSocialClasses
                .Select(cs => new ReferenceDto { Id = cs.SocialClass!.Id, Name = cs.SocialClass!.Name })));
    }
}
