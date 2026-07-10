using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.PoliticalIdeology;
using ChronicleKeeper.Core.Entities.Social.Politics;

public class PoliticalIdeologyProfile : Profile
{
    public PoliticalIdeologyProfile()
    {
        CreateMap<PoliticalIdeology, PoliticalIdeologyDto>();
        CreateMap<PoliticalIdeologyCreateDto, PoliticalIdeology>();
        CreateMap<PoliticalIdeologyUpdateDto, PoliticalIdeology>();

        CreateMap<PoliticalIdeology, PoliticalIdeologyDetailsDto>()
            .ForMember(dest => dest.AffiliatedPoliticalParties, opt => opt.MapFrom(src => src.AffiliatedPoliticalParties
                .Select(p => new ReferenceDto { Id = p.Id, Name = p.Name })))
            .ForMember(dest => dest.AffiliatedGovernmentSystems, opt => opt.MapFrom(src => src.AffiliatedGovernmentSystems
                .Select(g => new ReferenceDto { Id = g.Id, Name = g.Name })));
    }
}
