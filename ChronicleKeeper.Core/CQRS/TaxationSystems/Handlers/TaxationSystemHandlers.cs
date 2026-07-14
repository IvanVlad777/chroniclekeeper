using AutoMapper;
using ChronicleKeeper.Core.CQRS.TaxationSystems.Commands;
using ChronicleKeeper.Core.CQRS.TaxationSystems.Queries;
using ChronicleKeeper.Core.DTOs.TaxationSystem;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.TaxationSystems.Handlers
{
    public class GetAllTaxationSystemsQueryHandler : IRequestHandler<GetAllTaxationSystemsQuery, List<TaxationSystemDto>>
    {
        private readonly ITaxationSystemRepository _repository;
        private readonly IMapper _mapper;

        public GetAllTaxationSystemsQueryHandler(ITaxationSystemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<TaxationSystemDto>> Handle(GetAllTaxationSystemsQuery request, CancellationToken cancellationToken)
        {
            var taxationSystems = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<TaxationSystemDto>>(taxationSystems);
        }
    }

    public class GetTaxationSystemByIdQueryHandler : IRequestHandler<GetTaxationSystemByIdQuery, TaxationSystemDetailsDto?>
    {
        private readonly ITaxationSystemRepository _repository;
        private readonly IMapper _mapper;

        public GetTaxationSystemByIdQueryHandler(ITaxationSystemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TaxationSystemDetailsDto?> Handle(GetTaxationSystemByIdQuery request, CancellationToken cancellationToken)
        {
            var taxationSystem = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return taxationSystem == null ? null : _mapper.Map<TaxationSystemDetailsDto>(taxationSystem);
        }
    }

    public class CreateTaxationSystemCommandHandler : IRequestHandler<CreateTaxationSystemCommand, TaxationSystemDto>
    {
        private readonly ITaxationSystemRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTaxationSystemCommandHandler> _logger;

        public CreateTaxationSystemCommandHandler(
            ITaxationSystemRepository repository,
            IWorldRepository worldRepository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<CreateTaxationSystemCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TaxationSystemDto> Handle(CreateTaxationSystemCommand request, CancellationToken cancellationToken)
        {
            var dto = request.TaxationSystemCreateDto;
            _logger.LogInformation("Creating new taxation system: {Name}", dto.Name);

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

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Social.Economy.TaxationSystem>(dto), cancellationToken);
            return _mapper.Map<TaxationSystemDto>(created);
        }
    }

    public class UpdateTaxationSystemCommandHandler : IRequestHandler<UpdateTaxationSystemCommand, TaxationSystemDto>
    {
        private readonly ITaxationSystemRepository _repository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public UpdateTaxationSystemCommandHandler(
            ITaxationSystemRepository repository,
            IHistoryRepository historyRepository,
            IMapper mapper)
        {
            _repository = repository;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public async Task<TaxationSystemDto> Handle(UpdateTaxationSystemCommand request, CancellationToken cancellationToken)
        {
            var taxationSystem = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("TaxationSystem", request.Id);

            var dto = request.TaxationSystemUpdateDto;

            if (dto.HistoryId is int historyId)
            {
                var history = await _historyRepository.FindByIdAsync(historyId, cancellationToken)
                    ?? throw new DomainValidationException($"History with ID {historyId} does not exist.");
                if (history.WorldId != taxationSystem.WorldId)
                {
                    throw new DomainValidationException($"History with ID {historyId} does not belong to this world.");
                }
            }

            _mapper.Map(dto, taxationSystem);
            var updated = await _repository.UpdateAsync(taxationSystem, cancellationToken);
            return _mapper.Map<TaxationSystemDto>(updated);
        }
    }

    public class DeleteTaxationSystemCommandHandler : IRequestHandler<DeleteTaxationSystemCommand, bool>
    {
        private readonly ITaxationSystemRepository _repository;
        private readonly ILogger<DeleteTaxationSystemCommandHandler> _logger;

        public DeleteTaxationSystemCommandHandler(ITaxationSystemRepository repository, ILogger<DeleteTaxationSystemCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteTaxationSystemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting taxation system with ID {Id}", request.Id);

            var economicSystemsInUse = await _repository.CountEconomicSystemsUsingTaxationSystemAsync(request.Id, cancellationToken);
            if (economicSystemsInUse > 0)
            {
                throw new DomainValidationException(
                    $"This taxation system is used by {economicSystemsInUse} economic system(s). Reassign them first.");
            }

            var guildsInUse = await _repository.CountGuildsUsingTaxationSystemAsync(request.Id, cancellationToken);
            if (guildsInUse > 0)
            {
                throw new DomainValidationException(
                    $"This taxation system is used by {guildsInUse} guild(s). Reassign them first.");
            }

            var corporationsInUse = await _repository.CountCorporationsUsingTaxationSystemAsync(request.Id, cancellationToken);
            if (corporationsInUse > 0)
            {
                throw new DomainValidationException(
                    $"This taxation system is used by {corporationsInUse} corporation(s). Reassign them first.");
            }

            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
