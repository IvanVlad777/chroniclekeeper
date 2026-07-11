using AutoMapper;
using ChronicleKeeper.Core.DTOs;
using ChronicleKeeper.Core.DTOs.Timeline;
using ChronicleKeeper.Core.Entities.HistoryTimelines;

public class TimelineProfile : Profile
{
    public TimelineProfile()
    {
        CreateMap<Timeline, TimelineDto>();
        CreateMap<Timeline, TimelineDetailsDto>()
            .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History == null ? null : new ReferenceDto { Id = src.History.Id, Name = src.History.Name }));
        CreateMap<TimelineCreateDto, Timeline>();
        CreateMap<TimelineUpdateDto, Timeline>();

        CreateMap<TimelineEvent, TimelineEventDto>();
        CreateMap<TimelineEventCreateDto, TimelineEvent>();
        CreateMap<TimelineEventUpdateDto, TimelineEvent>();
    }
}
