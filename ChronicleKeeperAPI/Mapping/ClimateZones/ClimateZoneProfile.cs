using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.ClimateZone;
using ChronicleKeeper.Core.Entities.Geography.Climate;

public class ClimateZoneProfile : Profile
{
    public ClimateZoneProfile()
    {
        CreateMap<ClimateZone, ClimateZoneDto>();
        CreateMap<ClimateZoneCreateDto, ClimateZone>();
        CreateMap<ClimateZoneUpdateDto, ClimateZone>();

        CreateMap<ClimateZone, ClimateZoneDetailsDto>()
            .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History == null ? null : new ReferenceDto { Id = src.History.Id, Name = src.History.Name }))
            .ForMember(dest => dest.Climates, opt => opt.MapFrom(src => src.Climates
                .Select(zd => new ReferenceDto { Id = zd.ClimateDetail!.Id, Name = zd.ClimateDetail!.Name })))
            .ForMember(dest => dest.Seasons, opt => opt.MapFrom(src => src.Seasons
                .Select(zs => new ReferenceDto { Id = zs.Season!.Id, Name = zs.Season!.Name })))
            .ForMember(dest => dest.Locations, opt => opt.MapFrom(src => src.Locations
                .Select(lz => new ReferenceDto { Id = lz.Location!.Id, Name = lz.Location!.Name })))
            .ForMember(dest => dest.WeatherPatterns, opt => opt.MapFrom(src => src.TypicalWeatherPatterns));
    }
}
