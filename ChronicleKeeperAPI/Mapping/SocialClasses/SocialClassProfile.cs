using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.SocialClass;
using ChronicleKeeper.Core.Entities.Social.Structure;

public class SocialClassProfile : Profile
{
    public SocialClassProfile()
    {
        CreateMap<SocialClass, SocialClassDto>();
        CreateMap<SocialClassCreateDto, SocialClass>();
        CreateMap<SocialClassUpdateDto, SocialClass>();

        CreateMap<SocialClass, SocialClassDetailsDto>()
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Members
                .Select(m => new ReferenceDto { Id = m.Id, Name = m.Name })));
    }
}
