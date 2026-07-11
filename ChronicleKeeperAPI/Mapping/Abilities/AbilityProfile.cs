using AutoMapper;
using ChronicleKeeper.Core.DTOs.Ability;
using ChronicleKeeper.Core.Entities.Characters.Abilities;

public class AbilityProfile : Profile
{
    public AbilityProfile()
    {
        CreateMap<Ability, AbilityDto>();
        CreateMap<AbilityCreateDto, Ability>();
        CreateMap<AbilityUpdateDto, Ability>();
    }
}
