using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.History;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

public class HistoryProfile : Profile
{
    public HistoryProfile()
    {
        CreateMap<History, HistoryDto>();
        CreateMap<HistoryCreateDto, History>();
        CreateMap<HistoryUpdateDto, History>();

        CreateMap<History, HistoryDetailsDto>()
            .ForMember(dest => dest.Timelines, opt => opt.MapFrom(src => src.Timelines
                .Select(t => new ReferenceDto { Id = t.Id, Name = t.Name })));
    }
}
