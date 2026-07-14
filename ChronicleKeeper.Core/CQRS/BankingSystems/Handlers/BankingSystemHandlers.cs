using AutoMapper;
using ChronicleKeeper.Core.CQRS.BankingSystems.Commands;
using ChronicleKeeper.Core.CQRS.BankingSystems.Queries;
using ChronicleKeeper.Core.DTOs.BankingSystem;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.BankingSystems.Handlers
{
    public class GetAllBankingSystemsQueryHandler : IRequestHandler<GetAllBankingSystemsQuery, List<BankingSystemDto>>
    {
        private readonly IBankingSystemRepository _repository;
        private readonly IMapper _mapper;

        public GetAllBankingSystemsQueryHandler(IBankingSystemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<BankingSystemDto>> Handle(GetAllBankingSystemsQuery request, CancellationToken cancellationToken)
        {
            var bankingSystems = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<BankingSystemDto>>(bankingSystems);
        }
    }

    public class GetBankingSystemByIdQueryHandler : IRequestHandler<GetBankingSystemByIdQuery, BankingSystemDetailsDto?>
    {
        private readonly IBankingSystemRepository _repository;
        private readonly IMapper _mapper;

        public GetBankingSystemByIdQueryHandler(IBankingSystemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<BankingSystemDetailsDto?> Handle(GetBankingSystemByIdQuery request, CancellationToken cancellationToken)
        {
            var bankingSystem = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return bankingSystem == null ? null : _mapper.Map<BankingSystemDetailsDto>(bankingSystem);
        }
    }

    public class CreateBankingSystemCommandHandler : IRequestHandler<CreateBankingSystemCommand, BankingSystemDto>
    {
        private readonly IBankingSystemRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateBankingSystemCommandHandler> _logger;

        public CreateBankingSystemCommandHandler(
            IBankingSystemRepository repository,
            IWorldRepository worldRepository,
            ICurrencyRepository currencyRepository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<CreateBankingSystemCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _currencyRepository = currencyRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BankingSystemDto> Handle(CreateBankingSystemCommand request, CancellationToken cancellationToken)
        {
            var dto = request.BankingSystemCreateDto;
            _logger.LogInformation("Creating new banking system: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            await BankingSystemValidation.ValidateReferencesAsync(
                dto.CurrencyId, dto.HistoryId, dto.WorldId, _currencyRepository, _historyRepository, cancellationToken);

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Social.Economy.BankingSystem>(dto), cancellationToken);
            return _mapper.Map<BankingSystemDto>(created);
        }
    }

    public class UpdateBankingSystemCommandHandler : IRequestHandler<UpdateBankingSystemCommand, BankingSystemDto>
    {
        private readonly IBankingSystemRepository _repository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public UpdateBankingSystemCommandHandler(
            IBankingSystemRepository repository,
            ICurrencyRepository currencyRepository,
            IHistoryRepository historyRepository,
            IMapper mapper)
        {
            _repository = repository;
            _currencyRepository = currencyRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public async Task<BankingSystemDto> Handle(UpdateBankingSystemCommand request, CancellationToken cancellationToken)
        {
            var bankingSystem = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("BankingSystem", request.Id);

            var dto = request.BankingSystemUpdateDto;

            await BankingSystemValidation.ValidateReferencesAsync(
                dto.CurrencyId, dto.HistoryId, bankingSystem.WorldId, _currencyRepository, _historyRepository, cancellationToken);

            _mapper.Map(dto, bankingSystem);
            var updated = await _repository.UpdateAsync(bankingSystem, cancellationToken);
            return _mapper.Map<BankingSystemDto>(updated);
        }
    }

    public class DeleteBankingSystemCommandHandler : IRequestHandler<DeleteBankingSystemCommand, bool>
    {
        private readonly IBankingSystemRepository _repository;
        private readonly ILogger<DeleteBankingSystemCommandHandler> _logger;

        public DeleteBankingSystemCommandHandler(IBankingSystemRepository repository, ILogger<DeleteBankingSystemCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteBankingSystemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting banking system with ID {Id}", request.Id);

            var economicSystemsInUse = await _repository.CountEconomicSystemsUsingBankingSystemAsync(request.Id, cancellationToken);
            if (economicSystemsInUse > 0)
            {
                throw new DomainValidationException(
                    $"This banking system is used by {economicSystemsInUse} economic system(s). Reassign them first.");
            }

            var corporationsInUse = await _repository.CountCorporationsUsingBankingSystemAsync(request.Id, cancellationToken);
            if (corporationsInUse > 0)
            {
                throw new DomainValidationException(
                    $"This banking system is used by {corporationsInUse} corporation(s). Reassign them first.");
            }

            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    internal static class BankingSystemValidation
    {
        internal static async Task ValidateReferencesAsync(
            int? currencyId,
            int? historyId,
            int worldId,
            ICurrencyRepository currencyRepository,
            IHistoryRepository historyRepository,
            CancellationToken cancellationToken)
        {
            if (currencyId is int cid)
            {
                var currency = await currencyRepository.FindByIdAsync(cid, cancellationToken)
                    ?? throw new DomainValidationException($"Currency with ID {cid} does not exist.");
                if (currency.WorldId != worldId)
                {
                    throw new DomainValidationException($"Currency with ID {cid} does not belong to this world.");
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
