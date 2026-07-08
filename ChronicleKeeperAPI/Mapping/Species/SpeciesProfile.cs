using AutoMapper;
using ChronicleKeeper.Core.DTOs.Species;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;

public class SpeciesProfile : Profile
{
    public SpeciesProfile()
    {
        CreateMap<SapientSpecies, SpeciesDto>();
        CreateMap<SapientSpecies, SpeciesDetailsDto>();
        CreateMap<SpeciesCreateDto, SapientSpecies>();
        CreateMap<SpeciesUpdateDto, SapientSpecies>();

        CreateMap<Race, RaceDto>();
        CreateMap<RaceCreateDto, Race>();
        CreateMap<RaceUpdateDto, Race>();
    }
}
