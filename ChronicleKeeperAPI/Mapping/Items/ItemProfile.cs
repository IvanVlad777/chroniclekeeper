using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.Item;
using ChronicleKeeper.Core.Entities.Characters.Equipment;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<Item, ItemDto>();
        CreateMap<ItemCreateDto, Item>();
        CreateMap<ItemUpdateDto, Item>();

        CreateMap<Item, ItemDetailsDto>()
            .ForMember(dest => dest.CurrentOwner, opt => opt.MapFrom(src => src.CurrentOwner == null ? null : new ReferenceDto { Id = src.CurrentOwner.Id, Name = src.CurrentOwner.Name }))
            .ForMember(dest => dest.StoredAt, opt => opt.MapFrom(src => src.StoredAt == null ? null : new ReferenceDto { Id = src.StoredAt.Id, Name = src.StoredAt.Name }))
            .ForMember(dest => dest.Faction, opt => opt.MapFrom(src => src.Faction == null ? null : new ReferenceDto { Id = src.Faction.Id, Name = src.Faction.Name }))
            .ForMember(dest => dest.OwnershipHistory, opt => opt.MapFrom(src => src.OwnershipHistory));
    }
}
