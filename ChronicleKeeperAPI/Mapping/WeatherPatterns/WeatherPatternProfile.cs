using AutoMapper;
using ChronicleKeeper.Core.DTOs.WeatherPattern;
using ChronicleKeeper.Core.Entities.Geography.Climate;

public class WeatherPatternProfile : Profile
{
    public WeatherPatternProfile()
    {
        CreateMap<WeatherPattern, WeatherPatternDto>();
        CreateMap<WeatherPatternCreateDto, WeatherPattern>();
        CreateMap<WeatherPatternUpdateDto, WeatherPattern>();
    }
}
