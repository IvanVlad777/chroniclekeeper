using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.Hobby;
using ChronicleKeeper.Core.Entities.Characters.CharacterInfo;

public class HobbyProfile : Profile
{
    public HobbyProfile()
    {
        CreateMap<Hobby, HobbyDto>();
        CreateMap<HobbyCreateDto, Hobby>();
        CreateMap<HobbyUpdateDto, Hobby>();

        CreateMap<Hobby, HobbyDetailsDto>()
            .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History == null
                ? null
                : new ReferenceDto { Id = src.History.Id, Name = src.History.Name }))
            .ForMember(dest => dest.Practitioners, opt => opt.MapFrom(src => src.Practitioners
                .Where(x => x.Character != null)
                .Select(x => new ReferenceDto { Id = x.Character!.Id, Name = x.Character.Name })));
    }
}
