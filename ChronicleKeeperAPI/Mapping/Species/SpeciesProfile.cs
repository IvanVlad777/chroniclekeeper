using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.Species;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;

public class SpeciesProfile : Profile
{
    public SpeciesProfile()
    {
        CreateMap<SapientSpecies, SpeciesDto>();
        CreateMap<SapientSpecies, SpeciesDetailsDto>()
            .ForMember(dest => dest.ParentCreature, opt => opt.MapFrom(src => src.ParentCreature == null
                ? null
                : new ReferenceDto { Id = src.ParentCreature.Id, Name = src.ParentCreature.Name }))
            .ForMember(dest => dest.Subspecies, opt => opt.MapFrom(src => src.Subspecies
                .Select(c => new ReferenceDto { Id = c.Id, Name = c.Name })))
            .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History == null
                ? null
                : new ReferenceDto { Id = src.History.Id, Name = src.History.Name }));
        CreateMap<SpeciesCreateDto, SapientSpecies>();
        CreateMap<SpeciesUpdateDto, SapientSpecies>();

        CreateMap<Race, RaceDto>();
        CreateMap<RaceCreateDto, Race>();
        CreateMap<RaceUpdateDto, Race>();
    }
}
