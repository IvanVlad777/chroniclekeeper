using AutoMapper;
using ChronicleKeeper.Core.CQRS.Timelines.Commands;
using ChronicleKeeper.Core.CQRS.Timelines.Queries;
using ChronicleKeeper.Core.DTOs.Timeline;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Timelines.Handlers
{
    public class GetAllTimelinesQueryHandler : IRequestHandler<GetAllTimelinesQuery, List<TimelineDto>>
    {
        private readonly ITimelineRepository _repository;
        private readonly IMapper _mapper;

        public GetAllTimelinesQueryHandler(ITimelineRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<TimelineDto>> Handle(GetAllTimelinesQuery request, CancellationToken cancellationToken)
        {
            var timelines = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<TimelineDto>>(timelines);
        }
    }

    public class GetTimelineByIdQueryHandler : IRequestHandler<GetTimelineByIdQuery, TimelineDetailsDto?>
    {
        private readonly ITimelineRepository _repository;
        private readonly IMapper _mapper;

        public GetTimelineByIdQueryHandler(ITimelineRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TimelineDetailsDto?> Handle(GetTimelineByIdQuery request, CancellationToken cancellationToken)
        {
            var timeline = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return timeline == null ? null : _mapper.Map<TimelineDetailsDto>(timeline);
        }
    }

    public class CreateTimelineCommandHandler : IRequestHandler<CreateTimelineCommand, TimelineDto>
    {
        private readonly ITimelineRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTimelineCommandHandler> _logger;

        public CreateTimelineCommandHandler(
            ITimelineRepository repository,
            IWorldRepository worldRepository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<CreateTimelineCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TimelineDto> Handle(CreateTimelineCommand request, CancellationToken cancellationToken)
        {
            var dto = request.TimelineCreateDto;
            _logger.LogInformation("Creating new timeline: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            if (dto.HistoryId is int historyId)
            {
                var history = await _historyRepository.FindByIdAsync(historyId, cancellationToken)
                    ?? throw new DomainValidationException($"History with ID {historyId} does not exist.");
                if (history.WorldId != dto.WorldId)
                {
                    throw new DomainValidationException($"History with ID {historyId} does not belong to this world.");
                }
            }

            var created = await _repository.CreateAsync(_mapper.Map<Timeline>(dto), cancellationToken);
            return _mapper.Map<TimelineDto>(created);
        }
    }

    public class UpdateTimelineCommandHandler : IRequestHandler<UpdateTimelineCommand, TimelineDto>
    {
        private readonly ITimelineRepository _repository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public UpdateTimelineCommandHandler(ITimelineRepository repository, IHistoryRepository historyRepository, IMapper mapper)
        {
            _repository = repository;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public async Task<TimelineDto> Handle(UpdateTimelineCommand request, CancellationToken cancellationToken)
        {
            var timeline = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Timeline", request.Id);

            if (request.TimelineUpdateDto.HistoryId is int historyId)
            {
                var history = await _historyRepository.FindByIdAsync(historyId, cancellationToken)
                    ?? throw new DomainValidationException($"History with ID {historyId} does not exist.");
                if (history.WorldId != timeline.WorldId)
                {
                    throw new DomainValidationException($"History with ID {historyId} does not belong to this world.");
                }
            }

            _mapper.Map(request.TimelineUpdateDto, timeline);
            var updated = await _repository.UpdateAsync(timeline, cancellationToken);
            return _mapper.Map<TimelineDto>(updated);
        }
    }

    public class DeleteTimelineCommandHandler : IRequestHandler<DeleteTimelineCommand, bool>
    {
        private readonly ITimelineRepository _repository;
        private readonly ILogger<DeleteTimelineCommandHandler> _logger;

        public DeleteTimelineCommandHandler(ITimelineRepository repository, ILogger<DeleteTimelineCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteTimelineCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting timeline with ID {Id} (events cascade)", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    public class CreateTimelineEventCommandHandler : IRequestHandler<CreateTimelineEventCommand, TimelineEventDto>
    {
        private readonly ITimelineRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTimelineEventCommandHandler> _logger;

        public CreateTimelineEventCommandHandler(
            ITimelineRepository repository,
            IMapper mapper,
            ILogger<CreateTimelineEventCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TimelineEventDto> Handle(CreateTimelineEventCommand request, CancellationToken cancellationToken)
        {
            var timeline = await _repository.FindByIdAsync(request.TimelineId, cancellationToken)
                ?? throw new EntityNotFoundException("Timeline", request.TimelineId);

            var timelineEvent = _mapper.Map<TimelineEvent>(request.EventCreateDto);
            timelineEvent.TimelineId = timeline.Id;
            timelineEvent.WorldId = timeline.WorldId; // svijet eventa uvijek = svijet timelinea

            var created = await _repository.CreateEventAsync(timelineEvent, cancellationToken);
            _logger.LogInformation("Created event {EventId} on timeline {TimelineId}", created.Id, timeline.Id);

            return _mapper.Map<TimelineEventDto>(created);
        }
    }

    public class UpdateTimelineEventCommandHandler : IRequestHandler<UpdateTimelineEventCommand, TimelineEventDto>
    {
        private readonly ITimelineRepository _repository;
        private readonly IMapper _mapper;

        public UpdateTimelineEventCommandHandler(ITimelineRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TimelineEventDto> Handle(UpdateTimelineEventCommand request, CancellationToken cancellationToken)
        {
            var timelineEvent = await _repository.FindEventByIdAsync(request.EventId, cancellationToken)
                ?? throw new EntityNotFoundException("Timeline event", request.EventId);

            _mapper.Map(request.EventUpdateDto, timelineEvent);
            var updated = await _repository.UpdateEventAsync(timelineEvent, cancellationToken);
            return _mapper.Map<TimelineEventDto>(updated);
        }
    }

    public class DeleteTimelineEventCommandHandler : IRequestHandler<DeleteTimelineEventCommand, bool>
    {
        private readonly ITimelineRepository _repository;

        public DeleteTimelineEventCommandHandler(ITimelineRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteTimelineEventCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteEventAsync(request.EventId, cancellationToken);
        }
    }
}
