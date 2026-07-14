using AutoMapper;
using ChronicleKeeper.Core.CQRS.TradeRoutes.Commands;
using ChronicleKeeper.Core.CQRS.TradeRoutes.Queries;
using ChronicleKeeper.Core.DTOs.TradeRoute;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.TradeRoutes.Handlers
{
    public class GetAllTradeRoutesQueryHandler : IRequestHandler<GetAllTradeRoutesQuery, List<TradeRouteDto>>
    {
        private readonly ITradeRouteRepository _repository;
        private readonly IMapper _mapper;

        public GetAllTradeRoutesQueryHandler(ITradeRouteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<TradeRouteDto>> Handle(GetAllTradeRoutesQuery request, CancellationToken cancellationToken)
        {
            var tradeRoutes = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<TradeRouteDto>>(tradeRoutes);
        }
    }

    public class GetTradeRouteByIdQueryHandler : IRequestHandler<GetTradeRouteByIdQuery, TradeRouteDetailsDto?>
    {
        private readonly ITradeRouteRepository _repository;
        private readonly IMapper _mapper;

        public GetTradeRouteByIdQueryHandler(ITradeRouteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TradeRouteDetailsDto?> Handle(GetTradeRouteByIdQuery request, CancellationToken cancellationToken)
        {
            var tradeRoute = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return tradeRoute == null ? null : _mapper.Map<TradeRouteDetailsDto>(tradeRoute);
        }
    }

    public class CreateTradeRouteCommandHandler : IRequestHandler<CreateTradeRouteCommand, TradeRouteDto>
    {
        private readonly ITradeRouteRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTradeRouteCommandHandler> _logger;

        public CreateTradeRouteCommandHandler(
            ITradeRouteRepository repository,
            IWorldRepository worldRepository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<CreateTradeRouteCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TradeRouteDto> Handle(CreateTradeRouteCommand request, CancellationToken cancellationToken)
        {
            var dto = request.TradeRouteCreateDto;
            _logger.LogInformation("Creating new trade route: {Name}", dto.Name);

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

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Social.Economy.TradeRoute>(dto), cancellationToken);
            return _mapper.Map<TradeRouteDto>(created);
        }
    }

    public class UpdateTradeRouteCommandHandler : IRequestHandler<UpdateTradeRouteCommand, TradeRouteDto>
    {
        private readonly ITradeRouteRepository _repository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public UpdateTradeRouteCommandHandler(
            ITradeRouteRepository repository,
            IHistoryRepository historyRepository,
            IMapper mapper)
        {
            _repository = repository;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public async Task<TradeRouteDto> Handle(UpdateTradeRouteCommand request, CancellationToken cancellationToken)
        {
            var tradeRoute = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("TradeRoute", request.Id);

            var dto = request.TradeRouteUpdateDto;

            if (dto.HistoryId is int historyId)
            {
                var history = await _historyRepository.FindByIdAsync(historyId, cancellationToken)
                    ?? throw new DomainValidationException($"History with ID {historyId} does not exist.");
                if (history.WorldId != tradeRoute.WorldId)
                {
                    throw new DomainValidationException($"History with ID {historyId} does not belong to this world.");
                }
            }

            _mapper.Map(dto, tradeRoute);
            var updated = await _repository.UpdateAsync(tradeRoute, cancellationToken);
            return _mapper.Map<TradeRouteDto>(updated);
        }
    }

    public class DeleteTradeRouteCommandHandler : IRequestHandler<DeleteTradeRouteCommand, bool>
    {
        private readonly ITradeRouteRepository _repository;
        private readonly ILogger<DeleteTradeRouteCommandHandler> _logger;

        public DeleteTradeRouteCommandHandler(ITradeRouteRepository repository, ILogger<DeleteTradeRouteCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteTradeRouteCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting trade route with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    public class AddTradeRouteLocationCommandHandler : IRequestHandler<AddTradeRouteLocationCommand, bool>
    {
        private readonly ITradeRouteRepository _repository;
        private readonly ILocationRepository _locationRepository;

        public AddTradeRouteLocationCommandHandler(ITradeRouteRepository repository, ILocationRepository locationRepository)
        {
            _repository = repository;
            _locationRepository = locationRepository;
        }

        public async Task<bool> Handle(AddTradeRouteLocationCommand request, CancellationToken cancellationToken)
        {
            var tradeRoute = await _repository.FindByIdAsync(request.TradeRouteId, cancellationToken)
                ?? throw new EntityNotFoundException("TradeRoute", request.TradeRouteId);

            var location = await _locationRepository.FindByIdAsync(request.LocationId, cancellationToken)
                ?? throw new DomainValidationException($"Location with ID {request.LocationId} does not exist.");
            if (location.WorldId != tradeRoute.WorldId)
            {
                throw new DomainValidationException($"Location with ID {request.LocationId} does not belong to this world.");
            }

            if (await _repository.IsLocationLinkedAsync(request.TradeRouteId, request.LocationId, cancellationToken))
            {
                throw new DomainValidationException("This location is already linked to the trade route.");
            }

            await _repository.AddLocationAsync(request.TradeRouteId, request.LocationId, cancellationToken);
            return true;
        }
    }

    public class RemoveTradeRouteLocationCommandHandler : IRequestHandler<RemoveTradeRouteLocationCommand, bool>
    {
        private readonly ITradeRouteRepository _repository;

        public RemoveTradeRouteLocationCommandHandler(ITradeRouteRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(RemoveTradeRouteLocationCommand request, CancellationToken cancellationToken)
        {
            return _repository.RemoveLocationAsync(request.TradeRouteId, request.LocationId, cancellationToken);
        }
    }

    public class AddTradeRouteResourceCommandHandler : IRequestHandler<AddTradeRouteResourceCommand, bool>
    {
        private readonly ITradeRouteRepository _repository;
        private readonly INaturalResourceRepository _naturalResourceRepository;

        public AddTradeRouteResourceCommandHandler(ITradeRouteRepository repository, INaturalResourceRepository naturalResourceRepository)
        {
            _repository = repository;
            _naturalResourceRepository = naturalResourceRepository;
        }

        public async Task<bool> Handle(AddTradeRouteResourceCommand request, CancellationToken cancellationToken)
        {
            var tradeRoute = await _repository.FindByIdAsync(request.TradeRouteId, cancellationToken)
                ?? throw new EntityNotFoundException("TradeRoute", request.TradeRouteId);

            var naturalResource = await _naturalResourceRepository.FindByIdAsync(request.NaturalResourceId, cancellationToken)
                ?? throw new DomainValidationException($"Natural resource with ID {request.NaturalResourceId} does not exist.");
            if (naturalResource.WorldId != tradeRoute.WorldId)
            {
                throw new DomainValidationException($"Natural resource with ID {request.NaturalResourceId} does not belong to this world.");
            }

            if (await _repository.IsResourceLinkedAsync(request.TradeRouteId, request.NaturalResourceId, cancellationToken))
            {
                throw new DomainValidationException("This natural resource is already linked to the trade route.");
            }

            await _repository.AddResourceAsync(request.TradeRouteId, request.NaturalResourceId, cancellationToken);
            return true;
        }
    }

    public class RemoveTradeRouteResourceCommandHandler : IRequestHandler<RemoveTradeRouteResourceCommand, bool>
    {
        private readonly ITradeRouteRepository _repository;

        public RemoveTradeRouteResourceCommandHandler(ITradeRouteRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(RemoveTradeRouteResourceCommand request, CancellationToken cancellationToken)
        {
            return _repository.RemoveResourceAsync(request.TradeRouteId, request.NaturalResourceId, cancellationToken);
        }
    }
}
