using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.PoliticalParty;
using ChronicleKeeper.Core.Entities.Social.Politics;

public class PoliticalPartyProfile : Profile
{
    public PoliticalPartyProfile()
    {
        CreateMap<PoliticalParty, PoliticalPartyDto>();
        CreateMap<PoliticalPartyCreateDto, PoliticalParty>();
        CreateMap<PoliticalPartyUpdateDto, PoliticalParty>();

        CreateMap<PoliticalParty, PoliticalPartyDetailsDto>()
            .ForMember(dest => dest.PoliticalIdeology, opt => opt.MapFrom(src => new ReferenceDto { Id = src.PoliticalIdeology.Id, Name = src.PoliticalIdeology.Name }))
            .ForMember(dest => dest.GovernmentSystem, opt => opt.MapFrom(src => src.GovernmentSystem == null
                ? null
                : new ReferenceDto { Id = src.GovernmentSystem.Id, Name = src.GovernmentSystem.Name }))
            .ForMember(dest => dest.Factions, opt => opt.MapFrom(src => src.Factions
                .Select(pf => new ReferenceDto { Id = pf.Faction!.Id, Name = pf.Faction!.Name })))
            .ForMember(dest => dest.Nations, opt => opt.MapFrom(src => src.Nations
                .Select(pn => new ReferenceDto { Id = pn.Nation!.Id, Name = pn.Nation!.Name })));
    }
}
