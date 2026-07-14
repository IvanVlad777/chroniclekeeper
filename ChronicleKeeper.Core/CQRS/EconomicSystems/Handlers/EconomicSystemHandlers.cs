using AutoMapper;
using ChronicleKeeper.Core.CQRS.EconomicSystems.Commands;
using ChronicleKeeper.Core.CQRS.EconomicSystems.Queries;
using ChronicleKeeper.Core.DTOs.EconomicSystem;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.EconomicSystems.Handlers
{
    public class GetAllEconomicSystemsQueryHandler : IRequestHandler<GetAllEconomicSystemsQuery, List<EconomicSystemDto>>
    {
        private readonly IEconomicSystemRepository _repository;
        private readonly IMapper _mapper;

        public GetAllEconomicSystemsQueryHandler(IEconomicSystemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<EconomicSystemDto>> Handle(GetAllEconomicSystemsQuery request, CancellationToken cancellationToken)
        {
            var economicSystems = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<EconomicSystemDto>>(economicSystems);
        }
    }

    public class GetEconomicSystemByIdQueryHandler : IRequestHandler<GetEconomicSystemByIdQuery, EconomicSystemDetailsDto?>
    {
        private readonly IEconomicSystemRepository _repository;
        private readonly IMapper _mapper;

        public GetEconomicSystemByIdQueryHandler(IEconomicSystemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EconomicSystemDetailsDto?> Handle(GetEconomicSystemByIdQuery request, CancellationToken cancellationToken)
        {
            var economicSystem = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return economicSystem == null ? null : _mapper.Map<EconomicSystemDetailsDto>(economicSystem);
        }
    }

    public class CreateEconomicSystemCommandHandler : IRequestHandler<CreateEconomicSystemCommand, EconomicSystemDto>
    {
        private readonly IEconomicSystemRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly ITaxationSystemRepository _taxationSystemRepository;
        private readonly IBankingSystemRepository _bankingSystemRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateEconomicSystemCommandHandler> _logger;

        public CreateEconomicSystemCommandHandler(
            IEconomicSystemRepository repository,
            IWorldRepository worldRepository,
            ITaxationSystemRepository taxationSystemRepository,
            IBankingSystemRepository bankingSystemRepository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<CreateEconomicSystemCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _taxationSystemRepository = taxationSystemRepository;
            _bankingSystemRepository = bankingSystemRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<EconomicSystemDto> Handle(CreateEconomicSystemCommand request, CancellationToken cancellationToken)
        {
            var dto = request.EconomicSystemCreateDto;
            _logger.LogInformation("Creating new economic system: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            await EconomicSystemValidation.ValidateReferencesAsync(
                dto.TaxationSystemId, dto.BankingSystemId, dto.HistoryId, dto.WorldId,
                _taxationSystemRepository, _bankingSystemRepository, _historyRepository, cancellationToken);

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Social.Economy.EconomicSystem>(dto), cancellationToken);
            return _mapper.Map<EconomicSystemDto>(created);
        }
    }

    public class UpdateEconomicSystemCommandHandler : IRequestHandler<UpdateEconomicSystemCommand, EconomicSystemDto>
    {
        private readonly IEconomicSystemRepository _repository;
        private readonly ITaxationSystemRepository _taxationSystemRepository;
        private readonly IBankingSystemRepository _bankingSystemRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public UpdateEconomicSystemCommandHandler(
            IEconomicSystemRepository repository,
            ITaxationSystemRepository taxationSystemRepository,
            IBankingSystemRepository bankingSystemRepository,
            IHistoryRepository historyRepository,
            IMapper mapper)
        {
            _repository = repository;
            _taxationSystemRepository = taxationSystemRepository;
            _bankingSystemRepository = bankingSystemRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public async Task<EconomicSystemDto> Handle(UpdateEconomicSystemCommand request, CancellationToken cancellationToken)
        {
            var economicSystem = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("EconomicSystem", request.Id);

            var dto = request.EconomicSystemUpdateDto;

            await EconomicSystemValidation.ValidateReferencesAsync(
                dto.TaxationSystemId, dto.BankingSystemId, dto.HistoryId, economicSystem.WorldId,
                _taxationSystemRepository, _bankingSystemRepository, _historyRepository, cancellationToken);

            _mapper.Map(dto, economicSystem);
            var updated = await _repository.UpdateAsync(economicSystem, cancellationToken);
            return _mapper.Map<EconomicSystemDto>(updated);
        }
    }

    public class DeleteEconomicSystemCommandHandler : IRequestHandler<DeleteEconomicSystemCommand, bool>
    {
        private readonly IEconomicSystemRepository _repository;
        private readonly ILogger<DeleteEconomicSystemCommandHandler> _logger;

        public DeleteEconomicSystemCommandHandler(IEconomicSystemRepository repository, ILogger<DeleteEconomicSystemCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteEconomicSystemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting economic system with ID {Id}", request.Id);

            var locationsInUse = await _repository.CountLocationsUsingEconomicSystemAsync(request.Id, cancellationToken);
            if (locationsInUse > 0)
            {
                throw new DomainValidationException(
                    $"This economic system is used by {locationsInUse} location(s). Reassign them first.");
            }

            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    internal static class EconomicSystemValidation
    {
        internal static async Task ValidateReferencesAsync(
            int? taxationSystemId,
            int? bankingSystemId,
            int? historyId,
            int worldId,
            ITaxationSystemRepository taxationSystemRepository,
            IBankingSystemRepository bankingSystemRepository,
            IHistoryRepository historyRepository,
            CancellationToken cancellationToken)
        {
            if (taxationSystemId is int tid)
            {
                var taxationSystem = await taxationSystemRepository.FindByIdAsync(tid, cancellationToken)
                    ?? throw new DomainValidationException($"Taxation system with ID {tid} does not exist.");
                if (taxationSystem.WorldId != worldId)
                {
                    throw new DomainValidationException($"Taxation system with ID {tid} does not belong to this world.");
                }
            }

            if (bankingSystemId is int bid)
            {
                var bankingSystem = await bankingSystemRepository.FindByIdAsync(bid, cancellationToken)
                    ?? throw new DomainValidationException($"Banking system with ID {bid} does not exist.");
                if (bankingSystem.WorldId != worldId)
                {
                    throw new DomainValidationException($"Banking system with ID {bid} does not belong to this world.");
                }
            }

            if (historyId is int hid)
            {
                var history = await historyRepository.FindByIdAsync(hid, cancellationToken)
                    ?? throw new DomainValidationException($"History with ID {hid} does not exist.");
                if (history.WorldId != worldId)
                {
                    throw new DomainValidationException($"History with ID {hid} does not belong to this world.");
                }
            }
        }
    }
}
