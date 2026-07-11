using AutoMapper;
using ChronicleKeeper.Core.CQRS.Seasons.Commands;
using ChronicleKeeper.Core.CQRS.Seasons.Queries;
using ChronicleKeeper.Core.DTOs.Season;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Seasons.Handlers
{
    public class GetAllSeasonsQueryHandler : IRequestHandler<GetAllSeasonsQuery, List<SeasonDto>>
    {
        private readonly ISeasonRepository _repository;
        private readonly IMapper _mapper;

        public GetAllSeasonsQueryHandler(ISeasonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<SeasonDto>> Handle(GetAllSeasonsQuery request, CancellationToken cancellationToken)
        {
            var seasons = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<SeasonDto>>(seasons);
        }
    }

    public class GetSeasonByIdQueryHandler : IRequestHandler<GetSeasonByIdQuery, SeasonDto?>
    {
        private readonly ISeasonRepository _repository;
        private readonly IMapper _mapper;

        public GetSeasonByIdQueryHandler(ISeasonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SeasonDto?> Handle(GetSeasonByIdQuery request, CancellationToken cancellationToken)
        {
            var season = await _repository.FindByIdAsync(request.Id, cancellationToken);
            return season == null ? null : _mapper.Map<SeasonDto>(season);
        }
    }

    public class CreateSeasonCommandHandler : IRequestHandler<CreateSeasonCommand, SeasonDto>
    {
        private readonly ISeasonRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSeasonCommandHandler> _logger;

        public CreateSeasonCommandHandler(
            ISeasonRepository repository,
            IWorldRepository worldRepository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<CreateSeasonCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SeasonDto> Handle(CreateSeasonCommand request, CancellationToken cancellationToken)
        {
            var dto = request.SeasonCreateDto;
            _logger.LogInformation("Creating new season: {Name}", dto.Name);

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

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Geography.Climate.Season>(dto), cancellationToken);
            return _mapper.Map<SeasonDto>(created);
        }
    }

    public class UpdateSeasonCommandHandler : IRequestHandler<UpdateSeasonCommand, SeasonDto>
    {
        private readonly ISeasonRepository _repository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public UpdateSeasonCommandHandler(
            ISeasonRepository repository,
            IHistoryRepository historyRepository,
            IMapper mapper)
        {
            _repository = repository;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public async Task<SeasonDto> Handle(UpdateSeasonCommand request, CancellationToken cancellationToken)
        {
            var season = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Season", request.Id);

            var dto = request.SeasonUpdateDto;

            if (dto.HistoryId is int historyId)
            {
                var history = await _historyRepository.FindByIdAsync(historyId, cancellationToken)
                    ?? throw new DomainValidationException($"History with ID {historyId} does not exist.");
                if (history.WorldId != season.WorldId)
                {
                    throw new DomainValidationException($"History with ID {historyId} does not belong to this world.");
                }
            }

            _mapper.Map(dto, season);
            var updated = await _repository.UpdateAsync(season, cancellationToken);
            return _mapper.Map<SeasonDto>(updated);
        }
    }

    public class DeleteSeasonCommandHandler : IRequestHandler<DeleteSeasonCommand, bool>
    {
        private readonly ISeasonRepository _repository;
        private readonly ILogger<DeleteSeasonCommandHandler> _logger;

        public DeleteSeasonCommandHandler(ISeasonRepository repository, ILogger<DeleteSeasonCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteSeasonCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting season with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
