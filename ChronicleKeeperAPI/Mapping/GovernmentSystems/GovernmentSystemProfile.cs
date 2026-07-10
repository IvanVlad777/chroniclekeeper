using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.GovernmentSystem;
using ChronicleKeeper.Core.Entities.Social.Politics;

public class GovernmentSystemProfile : Profile
{
    public GovernmentSystemProfile()
    {
        CreateMap<GovernmentSystem, GovernmentSystemDto>();
        CreateMap<GovernmentSystemCreateDto, GovernmentSystem>();
        CreateMap<GovernmentSystemUpdateDto, GovernmentSystem>();

        CreateMap<GovernmentSystem, GovernmentSystemDetailsDto>()
            .ForMember(dest => dest.PoliticalIdeology, opt => opt.MapFrom(src => src.PoliticalIdeology == null
                ? null
                : new ReferenceDto { Id = src.PoliticalIdeology.Id, Name = src.PoliticalIdeology.Name }))
            .ForMember(dest => dest.PoliticalParties, opt => opt.MapFrom(src => src.PoliticalParties
                .Select(p => new ReferenceDto { Id = p.Id, Name = p.Name })));
    }
}
