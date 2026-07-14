using AutoMapper;
using ChronicleKeeper.Core.CQRS.Currencies.Commands;
using ChronicleKeeper.Core.CQRS.Currencies.Queries;
using ChronicleKeeper.Core.DTOs.Currency;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Currencies.Handlers
{
    public class GetAllCurrenciesQueryHandler : IRequestHandler<GetAllCurrenciesQuery, List<CurrencyDto>>
    {
        private readonly ICurrencyRepository _repository;
        private readonly IMapper _mapper;

        public GetAllCurrenciesQueryHandler(ICurrencyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<CurrencyDto>> Handle(GetAllCurrenciesQuery request, CancellationToken cancellationToken)
        {
            var currencies = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<CurrencyDto>>(currencies);
        }
    }

    public class GetCurrencyByIdQueryHandler : IRequestHandler<GetCurrencyByIdQuery, CurrencyDetailsDto?>
    {
        private readonly ICurrencyRepository _repository;
        private readonly IMapper _mapper;

        public GetCurrencyByIdQueryHandler(ICurrencyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CurrencyDetailsDto?> Handle(GetCurrencyByIdQuery request, CancellationToken cancellationToken)
        {
            var currency = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return currency == null ? null : _mapper.Map<CurrencyDetailsDto>(currency);
        }
    }

    public class CreateCurrencyCommandHandler : IRequestHandler<CreateCurrencyCommand, CurrencyDto>
    {
        private readonly ICurrencyRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCurrencyCommandHandler> _logger;

        public CreateCurrencyCommandHandler(
            ICurrencyRepository repository,
            IWorldRepository worldRepository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<CreateCurrencyCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CurrencyDto> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
        {
            var dto = request.CurrencyCreateDto;
            _logger.LogInformation("Creating new currency: {Name}", dto.Name);

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

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Social.Economy.Currency>(dto), cancellationToken);
            return _mapper.Map<CurrencyDto>(created);
        }
    }

    public class UpdateCurrencyCommandHandler : IRequestHandler<UpdateCurrencyCommand, CurrencyDto>
    {
        private readonly ICurrencyRepository _repository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public UpdateCurrencyCommandHandler(
            ICurrencyRepository repository,
            IHistoryRepository historyRepository,
            IMapper mapper)
        {
            _repository = repository;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public async Task<CurrencyDto> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
        {
            var currency = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Currency", request.Id);

            var dto = request.CurrencyUpdateDto;

            if (dto.HistoryId is int historyId)
            {
                var history = await _historyRepository.FindByIdAsync(historyId, cancellationToken)
                    ?? throw new DomainValidationException($"History with ID {historyId} does not exist.");
                if (history.WorldId != currency.WorldId)
                {
                    throw new DomainValidationException($"History with ID {historyId} does not belong to this world.");
                }
            }

            _mapper.Map(dto, currency);
            var updated = await _repository.UpdateAsync(currency, cancellationToken);
            return _mapper.Map<CurrencyDto>(updated);
        }
    }

    public class DeleteCurrencyCommandHandler : IRequestHandler<DeleteCurrencyCommand, bool>
    {
        private readonly ICurrencyRepository _repository;
        private readonly ILogger<DeleteCurrencyCommandHandler> _logger;

        public DeleteCurrencyCommandHandler(ICurrencyRepository repository, ILogger<DeleteCurrencyCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting currency with ID {Id}", request.Id);

            var bankingSystemsInUse = await _repository.CountBankingSystemsUsingCurrencyAsync(request.Id, cancellationToken);
            if (bankingSystemsInUse > 0)
            {
                throw new DomainValidationException(
                    $"This currency is used by {bankingSystemsInUse} banking system(s). Reassign them first.");
            }

            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
