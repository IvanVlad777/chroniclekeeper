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
                .Select(t => new HistoryTimelineDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    EventCount = t.Events.Count,
                    MajorEventCount = t.Events.Count(e => e.IsMajorEvent),
                    FirstDate = t.Events.OrderBy(e => e.SortOrder).Select(e => e.Date).FirstOrDefault() ?? string.Empty,
                    LastDate = t.Events.OrderByDescending(e => e.SortOrder).Select(e => e.Date).FirstOrDefault() ?? string.Empty,
                })))
            // LinkedEntities has no navigation from History — the query handler fills it.
            .ForMember(dest => dest.LinkedEntities, opt => opt.Ignore());
    }
}
