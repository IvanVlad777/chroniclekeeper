using ChronicleKeeper.Core.DTOs.Timeline;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Timelines.Commands
{
    public class CreateTimelineCommand : IRequest<TimelineDto>
    {
        public TimelineCreateDto TimelineCreateDto { get; set; } = new();
    }

    public class UpdateTimelineCommand : IRequest<TimelineDto>
    {
        public int Id { get; set; }
        public TimelineUpdateDto TimelineUpdateDto { get; set; } = new();
    }

    public class DeleteTimelineCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class CreateTimelineEventCommand : IRequest<TimelineEventDto>
    {
        public int TimelineId { get; set; }
        public TimelineEventCreateDto EventCreateDto { get; set; } = new();
    }

    public class UpdateTimelineEventCommand : IRequest<TimelineEventDto>
    {
        public int EventId { get; set; }
        public TimelineEventUpdateDto EventUpdateDto { get; set; } = new();
    }

    public class DeleteTimelineEventCommand : IRequest<bool>
    {
        public int EventId { get; set; }
    }
}
