using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.Religion;
using ChronicleKeeper.Core.Entities.Social.Religions;

public class ReligionProfile : Profile
{
    public ReligionProfile()
    {
        CreateMap<Religion, ReligionDto>();
        CreateMap<ReligionCreateDto, Religion>();
        CreateMap<ReligionUpdateDto, Religion>();

        CreateMap<Religion, ReligionDetailsDto>()
            .ForMember(dest => dest.Followers, opt => opt.MapFrom(src => src.Followers
                .Select(c => new ReferenceDto { Id = c.Id, Name = c.Name })));
    }
}
