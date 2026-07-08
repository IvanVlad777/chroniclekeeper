using ChronicleKeeper.Core.DTOs.Timeline;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Timelines.Queries
{
    public class GetAllTimelinesQuery : IRequest<List<TimelineDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetTimelineByIdQuery : IRequest<TimelineDetailsDto?>
    {
        public int Id { get; set; }
    }
}
