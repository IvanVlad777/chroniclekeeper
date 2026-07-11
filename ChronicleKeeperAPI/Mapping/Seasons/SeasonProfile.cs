using AutoMapper;
using ChronicleKeeper.Core.DTOs.Season;
using ChronicleKeeper.Core.Entities.Geography.Climate;

public class SeasonProfile : Profile
{
    public SeasonProfile()
    {
        CreateMap<Season, SeasonDto>();
        CreateMap<SeasonCreateDto, Season>();
        CreateMap<SeasonUpdateDto, Season>();
    }
}
