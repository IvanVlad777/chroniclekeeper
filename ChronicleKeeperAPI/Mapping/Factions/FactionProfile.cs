using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.Faction;
using ChronicleKeeper.Core.Entities.Social;

public class FactionProfile : Profile
{
    public FactionProfile()
    {
        CreateMap<Faction, FactionDto>();
        CreateMap<FactionCreateDto, Faction>();
        CreateMap<FactionUpdateDto, Faction>();

        CreateMap<FactionMember, FactionMemberDto>()
            .ForMember(dest => dest.CharacterName, opt => opt.MapFrom(src => src.Character == null ? string.Empty : src.Character.Name));

        CreateMap<Faction, FactionDetailsDto>()
            .ForMember(dest => dest.Leader, opt => opt.MapFrom(src => src.Leader == null
                ? null
                : new ReferenceDto { Id = src.Leader.Id, Name = src.Leader.Name }))
            .ForMember(dest => dest.Headquarters, opt => opt.MapFrom(src => src.Headquarters == null
                ? null
                : new ReferenceDto { Id = src.Headquarters.Id, Name = src.Headquarters.Name }))
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.Members))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags
                .Where(t => t.Tag != null)
                .Select(t => new ReferenceDto { Id = t.Tag!.Id, Name = t.Tag.Name })))
            .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History == null
                ? null
                : new ReferenceDto { Id = src.History.Id, Name = src.History.Name }));
    }
}
