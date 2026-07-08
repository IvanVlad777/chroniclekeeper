using AutoMapper;
using ChronicleKeeper.Core.DTOs.World;
using ChronicleKeeper.Core.Entities.Worlds;

public class WorldProfile : Profile
{
    public WorldProfile()
    {
        CreateMap<World, WorldDto>();
        CreateMap<WorldCreateDto, World>();
        CreateMap<WorldUpdateDto, World>();
    }
}
