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
                : new ReferenceDto { Id = src.History.Id, Name = src.History.Name }))
            .ForMember(dest => dest.FrequentOccupations, opt => opt.MapFrom(src => src.FrequentOccupations
                .Where(x => x.Profession != null)
                .Select(x => new ReferenceDto { Id = x.Profession!.Id, Name = x.Profession.Name })))
            .ForMember(dest => dest.Cultures, opt => opt.MapFrom(src => src.Cultures
                .Where(x => x.Culture != null)
                .Select(x => new ReferenceDto { Id = x.Culture!.Id, Name = x.Culture.Name })))
            .ForMember(dest => dest.Folklore, opt => opt.MapFrom(src => src.Folklore
                .Where(x => x.Folklore != null)
                .Select(x => new ReferenceDto { Id = x.Folklore!.Id, Name = x.Folklore.Name })));
        CreateMap<SpeciesCreateDto, SapientSpecies>();
        CreateMap<SpeciesUpdateDto, SapientSpecies>();

        CreateMap<Race, RaceDto>();
        CreateMap<RaceCreateDto, Race>();
        CreateMap<RaceUpdateDto, Race>();
    }
}
