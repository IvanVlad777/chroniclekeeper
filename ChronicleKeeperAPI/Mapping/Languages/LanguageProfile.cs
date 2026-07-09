using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.Language;
using ChronicleKeeper.Core.Entities.Social.Cultures;

public class LanguageProfile : Profile
{
    public LanguageProfile()
    {
        CreateMap<Language, LanguageDto>();
        CreateMap<LanguageCreateDto, Language>();
        CreateMap<LanguageUpdateDto, Language>();

        CreateMap<Language, LanguageDetailsDto>()
            .ForMember(dest => dest.Cultures, opt => opt.MapFrom(src => src.Cultures
                .Select(c => new ReferenceDto { Id = c.Id, Name = c.Name })));
    }
}
