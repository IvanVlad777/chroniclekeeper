using AutoMapper;
using ChronicleKeeper.Core.DTOs.AbilityLevel;
using ChronicleKeeper.Core.Entities.Characters.Abilities;

public class AbilityLevelProfile : Profile
{
    public AbilityLevelProfile()
    {
        CreateMap<AbilityLevel, AbilityLevelDto>();
        CreateMap<AbilityLevelCreateDto, AbilityLevel>();
        CreateMap<AbilityLevelUpdateDto, AbilityLevel>();
    }
}
