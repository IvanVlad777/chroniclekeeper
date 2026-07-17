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

        CreateMap<TimelineEvent, TimelineEventDto>()
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src =>
                src.Location == null ? null : new ReferenceDto { Id = src.Location.Id, Name = src.Location.Name }))
            .ForMember(dest => dest.InvolvedCharacters, opt => opt.MapFrom(src =>
                src.InvolvedCharacters.Select(ic => new ReferenceDto { Id = ic.CharacterId, Name = ic.Character!.Name })));
        // Scalars (incl. LocationId) map by name; the join collection is synced in the handler/repo.
        CreateMap<TimelineEventCreateDto, TimelineEvent>()
            .ForMember(dest => dest.InvolvedCharacters, opt => opt.Ignore());
        CreateMap<TimelineEventUpdateDto, TimelineEvent>()
            .ForMember(dest => dest.InvolvedCharacters, opt => opt.Ignore());
    }
}
