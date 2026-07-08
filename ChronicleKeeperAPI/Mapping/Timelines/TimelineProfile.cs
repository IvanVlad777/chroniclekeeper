using AutoMapper;
using ChronicleKeeper.Core.DTOs.Timeline;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

public class TimelineProfile : Profile
{
    public TimelineProfile()
    {
        CreateMap<Timeline, TimelineDto>();
        CreateMap<Timeline, TimelineDetailsDto>();
        CreateMap<TimelineCreateDto, Timeline>();
        CreateMap<TimelineUpdateDto, Timeline>();

        CreateMap<TimelineEvent, TimelineEventDto>();
        CreateMap<TimelineEventCreateDto, TimelineEvent>();
        CreateMap<TimelineEventUpdateDto, TimelineEvent>();
    }
}
