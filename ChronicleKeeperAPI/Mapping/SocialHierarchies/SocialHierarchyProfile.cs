using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.SocialHierarchy;
using ChronicleKeeper.Core.Entities.Social.Structure;

public class SocialHierarchyProfile : Profile
{
    public SocialHierarchyProfile()
    {
        CreateMap<SocialHierarchy, SocialHierarchyDto>();
        CreateMap<SocialHierarchyCreateDto, SocialHierarchy>();
        CreateMap<SocialHierarchyUpdateDto, SocialHierarchy>();

        CreateMap<SocialHierarchy, SocialHierarchyDetailsDto>()
            .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History == null
                ? null
                : new ReferenceDto { Id = src.History.Id, Name = src.History.Name }))
            .ForMember(dest => dest.Classes, opt => opt.MapFrom(src => src.Classes
                .Select(c => new ReferenceDto { Id = c.Id, Name = c.Name })))
            .ForMember(dest => dest.Nations, opt => opt.MapFrom(src => src.Nations
                .Select(n => new ReferenceDto { Id = n.Id, Name = n.Name })));
    }
}
