using AutoMapper;
using ChronicleKeeper.Core.CQRS.WeatherPatterns.Commands;
using ChronicleKeeper.Core.CQRS.WeatherPatterns.Queries;
using ChronicleKeeper.Core.DTOs.WeatherPattern;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.WeatherPatterns.Handlers
{
    public class GetAllWeatherPatternsQueryHandler : IRequestHandler<GetAllWeatherPatternsQuery, List<WeatherPatternDto>>
    {
        private readonly IWeatherPatternRepository _repository;
        private readonly IMapper _mapper;

        public GetAllWeatherPatternsQueryHandler(IWeatherPatternRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<WeatherPatternDto>> Handle(GetAllWeatherPatternsQuery request, CancellationToken cancellationToken)
        {
            var weatherPatterns = await _repository.GetAllAsync(request.WorldId, request.ClimateZoneId, cancellationToken);
            return _mapper.Map<List<WeatherPatternDto>>(weatherPatterns);
        }
    }

    public class GetWeatherPatternByIdQueryHandler : IRequestHandler<GetWeatherPatternByIdQuery, WeatherPatternDto?>
    {
        private readonly IWeatherPatternRepository _repository;
        private readonly IMapper _mapper;

        public GetWeatherPatternByIdQueryHandler(IWeatherPatternRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<WeatherPatternDto?> Handle(GetWeatherPatternByIdQuery request, CancellationToken cancellationToken)
        {
            var weatherPattern = await _repository.FindByIdAsync(request.Id, cancellationToken);
            return weatherPattern == null ? null : _mapper.Map<WeatherPatternDto>(weatherPattern);
        }
    }

    public class CreateWeatherPatternCommandHandler : IRequestHandler<CreateWeatherPatternCommand, WeatherPatternDto>
    {
        private readonly IWeatherPatternRepository _repository;
        private readonly IClimateZoneRepository _climateZoneRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateWeatherPatternCommandHandler> _logger;

        public CreateWeatherPatternCommandHandler(
            IWeatherPatternRepository repository,
            IClimateZoneRepository climateZoneRepository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<CreateWeatherPatternCommandHandler> logger)
        {
            _repository = repository;
            _climateZoneRepository = climateZoneRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<WeatherPatternDto> Handle(CreateWeatherPatternCommand request, CancellationToken cancellationToken)
        {
            var dto = request.WeatherPatternCreateDto;
            _logger.LogInformation("Creating new weather pattern: {Name}", dto.Name);

            var climateZone = await _climateZoneRepository.FindByIdAsync(dto.ClimateZoneId, cancellationToken)
                ?? throw new DomainValidationException($"Climate zone with ID {dto.ClimateZoneId} does not exist.");

            if (dto.HistoryId is int historyId)
            {
                var history = await _historyRepository.FindByIdAsync(historyId, cancellationToken)
                    ?? throw new DomainValidationException($"History with ID {historyId} does not exist.");
                if (history.WorldId != climateZone.WorldId)
                {
                    throw new DomainValidationException($"History with ID {historyId} does not belong to this world.");
                }
            }

            var weatherPattern = _mapper.Map<Entities.Geography.Climate.WeatherPattern>(dto);
            weatherPattern.WorldId = climateZone.WorldId; // svijet obrasca uvijek = svijet klimatske zone

            var created = await _repository.CreateAsync(weatherPattern, cancellationToken);
            return _mapper.Map<WeatherPatternDto>(created);
        }
    }

    public class UpdateWeatherPatternCommandHandler : IRequestHandler<UpdateWeatherPatternCommand, WeatherPatternDto>
    {
        private readonly IWeatherPatternRepository _repository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public UpdateWeatherPatternCommandHandler(
            IWeatherPatternRepository repository,
            IHistoryRepository historyRepository,
            IMapper mapper)
        {
            _repository = repository;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public async Task<WeatherPatternDto> Handle(UpdateWeatherPatternCommand request, CancellationToken cancellationToken)
        {
            var weatherPattern = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("WeatherPattern", request.Id);

            var dto = request.WeatherPatternUpdateDto;

            if (dto.HistoryId is int historyId)
            {
                var history = await _historyRepository.FindByIdAsync(historyId, cancellationToken)
                    ?? throw new DomainValidationException($"History with ID {historyId} does not exist.");
                if (history.WorldId != weatherPattern.WorldId)
                {
                    throw new DomainValidationException($"History with ID {historyId} does not belong to this world.");
                }
            }

            _mapper.Map(dto, weatherPattern);
            var updated = await _repository.UpdateAsync(weatherPattern, cancellationToken);
            return _mapper.Map<WeatherPatternDto>(updated);
        }
    }

    public class DeleteWeatherPatternCommandHandler : IRequestHandler<DeleteWeatherPatternCommand, bool>
    {
        private readonly IWeatherPatternRepository _repository;
        private readonly ILogger<DeleteWeatherPatternCommandHandler> _logger;

        public DeleteWeatherPatternCommandHandler(IWeatherPatternRepository repository, ILogger<DeleteWeatherPatternCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteWeatherPatternCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting weather pattern with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
