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
            .ForMember(dest => dest.Religion, opt => opt.MapFrom(src => src.Religion == null ? null : new ReferenceDto { Id = src.Religion.Id, Name = src.Religion.Name }));
    }
}
