using AutoMapper;
using ChronicleKeeper.Core.CQRS.ClimateZones.Commands;
using ChronicleKeeper.Core.CQRS.ClimateZones.Queries;
using ChronicleKeeper.Core.DTOs.ClimateZone;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.ClimateZones.Handlers
{
    public class GetAllClimateZonesQueryHandler : IRequestHandler<GetAllClimateZonesQuery, List<ClimateZoneDto>>
    {
        private readonly IClimateZoneRepository _repository;
        private readonly IMapper _mapper;

        public GetAllClimateZonesQueryHandler(IClimateZoneRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ClimateZoneDto>> Handle(GetAllClimateZonesQuery request, CancellationToken cancellationToken)
        {
            var climateZones = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<ClimateZoneDto>>(climateZones);
        }
    }

    public class GetClimateZoneByIdQueryHandler : IRequestHandler<GetClimateZoneByIdQuery, ClimateZoneDetailsDto?>
    {
        private readonly IClimateZoneRepository _repository;
        private readonly IMapper _mapper;

        public GetClimateZoneByIdQueryHandler(IClimateZoneRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ClimateZoneDetailsDto?> Handle(GetClimateZoneByIdQuery request, CancellationToken cancellationToken)
        {
            var climateZone = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return climateZone == null ? null : _mapper.Map<ClimateZoneDetailsDto>(climateZone);
        }
    }

    public class CreateClimateZoneCommandHandler : IRequestHandler<CreateClimateZoneCommand, ClimateZoneDto>
    {
        private readonly IClimateZoneRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateClimateZoneCommandHandler> _logger;

        public CreateClimateZoneCommandHandler(
            IClimateZoneRepository repository,
            IWorldRepository worldRepository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<CreateClimateZoneCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ClimateZoneDto> Handle(CreateClimateZoneCommand request, CancellationToken cancellationToken)
        {
            var dto = request.ClimateZoneCreateDto;
            _logger.LogInformation("Creating new climate zone: {Name}", dto.Name);

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

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Geography.Climate.ClimateZone>(dto), cancellationToken);
            return _mapper.Map<ClimateZoneDto>(created);
        }
    }

    public class UpdateClimateZoneCommandHandler : IRequestHandler<UpdateClimateZoneCommand, ClimateZoneDto>
    {
        private readonly IClimateZoneRepository _repository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public UpdateClimateZoneCommandHandler(
            IClimateZoneRepository repository,
            IHistoryRepository historyRepository,
            IMapper mapper)
        {
            _repository = repository;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public async Task<ClimateZoneDto> Handle(UpdateClimateZoneCommand request, CancellationToken cancellationToken)
        {
            var climateZone = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("ClimateZone", request.Id);

            var dto = request.ClimateZoneUpdateDto;

            if (dto.HistoryId is int historyId)
            {
                var history = await _historyRepository.FindByIdAsync(historyId, cancellationToken)
                    ?? throw new DomainValidationException($"History with ID {historyId} does not exist.");
                if (history.WorldId != climateZone.WorldId)
                {
                    throw new DomainValidationException($"History with ID {historyId} does not belong to this world.");
                }
            }

            _mapper.Map(dto, climateZone);
            var updated = await _repository.UpdateAsync(climateZone, cancellationToken);
            return _mapper.Map<ClimateZoneDto>(updated);
        }
    }

    public class DeleteClimateZoneCommandHandler : IRequestHandler<DeleteClimateZoneCommand, bool>
    {
        private readonly IClimateZoneRepository _repository;
        private readonly ILogger<DeleteClimateZoneCommandHandler> _logger;

        public DeleteClimateZoneCommandHandler(IClimateZoneRepository repository, ILogger<DeleteClimateZoneCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteClimateZoneCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting climate zone with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    public class AddClimateZoneDetailCommandHandler : IRequestHandler<AddClimateZoneDetailCommand, bool>
    {
        private readonly IClimateZoneRepository _repository;
        private readonly IClimateDetailRepository _climateDetailRepository;

        public AddClimateZoneDetailCommandHandler(IClimateZoneRepository repository, IClimateDetailRepository climateDetailRepository)
        {
            _repository = repository;
            _climateDetailRepository = climateDetailRepository;
        }

        public async Task<bool> Handle(AddClimateZoneDetailCommand request, CancellationToken cancellationToken)
        {
            var climateZone = await _repository.FindByIdAsync(request.ClimateZoneId, cancellationToken)
                ?? throw new EntityNotFoundException("ClimateZone", request.ClimateZoneId);

            var climateDetail = await _climateDetailRepository.FindByIdAsync(request.ClimateDetailId, cancellationToken)
                ?? throw new DomainValidationException($"Climate detail with ID {request.ClimateDetailId} does not exist.");
            if (climateDetail.WorldId != climateZone.WorldId)
            {
                throw new DomainValidationException($"Climate detail with ID {request.ClimateDetailId} does not belong to this world.");
            }

            if (await _repository.IsClimateDetailLinkedAsync(request.ClimateZoneId, request.ClimateDetailId, cancellationToken))
            {
                throw new DomainValidationException("This climate detail is already linked to the climate zone.");
            }

            await _repository.AddClimateDetailAsync(request.ClimateZoneId, request.ClimateDetailId, cancellationToken);
            return true;
        }
    }

    public class RemoveClimateZoneDetailCommandHandler : IRequestHandler<RemoveClimateZoneDetailCommand, bool>
    {
        private readonly IClimateZoneRepository _repository;

        public RemoveClimateZoneDetailCommandHandler(IClimateZoneRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(RemoveClimateZoneDetailCommand request, CancellationToken cancellationToken)
        {
            return _repository.RemoveClimateDetailAsync(request.ClimateZoneId, request.ClimateDetailId, cancellationToken);
        }
    }

    public class AddClimateZoneSeasonCommandHandler : IRequestHandler<AddClimateZoneSeasonCommand, bool>
    {
        private readonly IClimateZoneRepository _repository;
        private readonly ISeasonRepository _seasonRepository;

        public AddClimateZoneSeasonCommandHandler(IClimateZoneRepository repository, ISeasonRepository seasonRepository)
        {
            _repository = repository;
            _seasonRepository = seasonRepository;
        }

        public async Task<bool> Handle(AddClimateZoneSeasonCommand request, CancellationToken cancellationToken)
        {
            var climateZone = await _repository.FindByIdAsync(request.ClimateZoneId, cancellationToken)
                ?? throw new EntityNotFoundException("ClimateZone", request.ClimateZoneId);

            var season = await _seasonRepository.FindByIdAsync(request.SeasonId, cancellationToken)
                ?? throw new DomainValidationException($"Season with ID {request.SeasonId} does not exist.");
            if (season.WorldId != climateZone.WorldId)
            {
                throw new DomainValidationException($"Season with ID {request.SeasonId} does not belong to this world.");
            }

            if (await _repository.IsSeasonLinkedAsync(request.ClimateZoneId, request.SeasonId, cancellationToken))
            {
                throw new DomainValidationException("This season is already linked to the climate zone.");
            }

            await _repository.AddSeasonAsync(request.ClimateZoneId, request.SeasonId, cancellationToken);
            return true;
        }
    }

    public class RemoveClimateZoneSeasonCommandHandler : IRequestHandler<RemoveClimateZoneSeasonCommand, bool>
    {
        private readonly IClimateZoneRepository _repository;

        public RemoveClimateZoneSeasonCommandHandler(IClimateZoneRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(RemoveClimateZoneSeasonCommand request, CancellationToken cancellationToken)
        {
            return _repository.RemoveSeasonAsync(request.ClimateZoneId, request.SeasonId, cancellationToken);
        }
    }

    public class AddClimateZoneLocationCommandHandler : IRequestHandler<AddClimateZoneLocationCommand, bool>
    {
        private readonly IClimateZoneRepository _repository;
        private readonly ILocationRepository _locationRepository;

        public AddClimateZoneLocationCommandHandler(IClimateZoneRepository repository, ILocationRepository locationRepository)
        {
            _repository = repository;
            _locationRepository = locationRepository;
        }

        public async Task<bool> Handle(AddClimateZoneLocationCommand request, CancellationToken cancellationToken)
        {
            var climateZone = await _repository.FindByIdAsync(request.ClimateZoneId, cancellationToken)
                ?? throw new EntityNotFoundException("ClimateZone", request.ClimateZoneId);

            var location = await _locationRepository.FindByIdAsync(request.LocationId, cancellationToken)
                ?? throw new DomainValidationException($"Location with ID {request.LocationId} does not exist.");
            if (location.WorldId != climateZone.WorldId)
            {
                throw new DomainValidationException($"Location with ID {request.LocationId} does not belong to this world.");
            }

            if (await _repository.IsLocationLinkedAsync(request.ClimateZoneId, request.LocationId, cancellationToken))
            {
                throw new DomainValidationException("This location is already linked to the climate zone.");
            }

            await _repository.AddLocationAsync(request.ClimateZoneId, request.LocationId, cancellationToken);
            return true;
        }
    }

    public class RemoveClimateZoneLocationCommandHandler : IRequestHandler<RemoveClimateZoneLocationCommand, bool>
    {
        private readonly IClimateZoneRepository _repository;

        public RemoveClimateZoneLocationCommandHandler(IClimateZoneRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(RemoveClimateZoneLocationCommand request, CancellationToken cancellationToken)
        {
            return _repository.RemoveLocationAsync(request.ClimateZoneId, request.LocationId, cancellationToken);
        }
    }
}
