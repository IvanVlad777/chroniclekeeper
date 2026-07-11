using AutoMapper;
using ChronicleKeeper.Core.DTOs.OwnershipHistory;
using ChronicleKeeper.Core.Entities.Characters.Equipment;

public class OwnershipHistoryProfile : Profile
{
    public OwnershipHistoryProfile()
    {
        CreateMap<OwnershipHistory, OwnershipHistoryDto>();
        CreateMap<OwnershipHistoryCreateDto, OwnershipHistory>();
        CreateMap<OwnershipHistoryUpdateDto, OwnershipHistory>();
    }
}
