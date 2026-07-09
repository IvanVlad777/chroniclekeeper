using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.Nation;
using ChronicleKeeper.Core.Entities.Social.Nationality;

public class NationProfile : Profile
{
    public NationProfile()
    {
        CreateMap<Nation, NationDto>();
        CreateMap<NationCreateDto, Nation>();
        CreateMap<NationUpdateDto, Nation>();

        CreateMap<Nation, NationDetailsDto>()
            .ForMember(dest => dest.Citizens, opt => opt.MapFrom(src => src.Characters
                .Select(c => new ReferenceDto { Id = c.Id, Name = c.Name })));
    }
}
