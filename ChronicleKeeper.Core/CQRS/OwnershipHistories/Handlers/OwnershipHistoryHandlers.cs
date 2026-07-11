using AutoMapper;
using ChronicleKeeper.Core.CQRS.OwnershipHistories.Commands;
using ChronicleKeeper.Core.CQRS.OwnershipHistories.Queries;
using ChronicleKeeper.Core.DTOs.OwnershipHistory;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.OwnershipHistories.Handlers
{
    public class GetAllOwnershipHistoriesQueryHandler : IRequestHandler<GetAllOwnershipHistoriesQuery, List<OwnershipHistoryDto>>
    {
        private readonly IOwnershipHistoryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllOwnershipHistoriesQueryHandler(IOwnershipHistoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<OwnershipHistoryDto>> Handle(GetAllOwnershipHistoriesQuery request, CancellationToken cancellationToken)
        {
            var histories = await _repository.GetAllAsync(request.WorldId, request.ItemId, cancellationToken);
            return _mapper.Map<List<OwnershipHistoryDto>>(histories);
        }
    }

    public class GetOwnershipHistoryByIdQueryHandler : IRequestHandler<GetOwnershipHistoryByIdQuery, OwnershipHistoryDto?>
    {
        private readonly IOwnershipHistoryRepository _repository;
        private readonly IMapper _mapper;

        public GetOwnershipHistoryByIdQueryHandler(IOwnershipHistoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OwnershipHistoryDto?> Handle(GetOwnershipHistoryByIdQuery request, CancellationToken cancellationToken)
        {
            var history = await _repository.FindByIdAsync(request.Id, cancellationToken);
            return history == null ? null : _mapper.Map<OwnershipHistoryDto>(history);
        }
    }

    public class CreateOwnershipHistoryCommandHandler : IRequestHandler<CreateOwnershipHistoryCommand, OwnershipHistoryDto>
    {
        private readonly IOwnershipHistoryRepository _repository;
        private readonly IItemRepository _itemRepository;
        private readonly ICharacterRepository _characterRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateOwnershipHistoryCommandHandler> _logger;

        public CreateOwnershipHistoryCommandHandler(
            IOwnershipHistoryRepository repository,
            IItemRepository itemRepository,
            ICharacterRepository characterRepository,
            IMapper mapper,
            ILogger<CreateOwnershipHistoryCommandHandler> logger)
        {
            _repository = repository;
            _itemRepository = itemRepository;
            _characterRepository = characterRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OwnershipHistoryDto> Handle(CreateOwnershipHistoryCommand request, CancellationToken cancellationToken)
        {
            var dto = request.OwnershipHistoryCreateDto;
            _logger.LogInformation("Creating new ownership history: {Name}", dto.Name);

            var item = await _itemRepository.FindByIdAsync(dto.ItemId, cancellationToken)
                ?? throw new DomainValidationException($"Item with ID {dto.ItemId} does not exist.");

            var history = _mapper.Map<Entities.Characters.Equipment.OwnershipHistory>(dto);
            history.WorldId = item.WorldId; // svijet zapisa uvijek = svijet predmeta

            await OwnershipHistoryValidation.ValidateReferencesAsync(_characterRepository, history, cancellationToken);

            var created = await _repository.CreateAsync(history, cancellationToken);
            return _mapper.Map<OwnershipHistoryDto>(created);
        }
    }

    public class UpdateOwnershipHistoryCommandHandler : IRequestHandler<UpdateOwnershipHistoryCommand, OwnershipHistoryDto>
    {
        private readonly IOwnershipHistoryRepository _repository;
        private readonly ICharacterRepository _characterRepository;
        private readonly IMapper _mapper;

        public UpdateOwnershipHistoryCommandHandler(
            IOwnershipHistoryRepository repository,
            ICharacterRepository characterRepository,
            IMapper mapper)
        {
            _repository = repository;
            _characterRepository = characterRepository;
            _mapper = mapper;
        }

        public async Task<OwnershipHistoryDto> Handle(UpdateOwnershipHistoryCommand request, CancellationToken cancellationToken)
        {
            var history = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("OwnershipHistory", request.Id);

            _mapper.Map(request.OwnershipHistoryUpdateDto, history);
            await OwnershipHistoryValidation.ValidateReferencesAsync(_characterRepository, history, cancellationToken);

            var updated = await _repository.UpdateAsync(history, cancellationToken);
            return _mapper.Map<OwnershipHistoryDto>(updated);
        }
    }

    public class DeleteOwnershipHistoryCommandHandler : IRequestHandler<DeleteOwnershipHistoryCommand, bool>
    {
        private readonly IOwnershipHistoryRepository _repository;
        private readonly ILogger<DeleteOwnershipHistoryCommandHandler> _logger;

        public DeleteOwnershipHistoryCommandHandler(IOwnershipHistoryRepository repository, ILogger<DeleteOwnershipHistoryCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteOwnershipHistoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting ownership history with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    internal static class OwnershipHistoryValidation
    {
        /// <summary>
        /// Prethodni i novi vlasnik moraju postojati U ISTOM SVIJETU kao zapis (svijet zapisa = svijet predmeta) —
        /// cross-world reference bi kasnije blokirale brisanje tuđeg svijeta.
        /// </summary>
        public static async Task ValidateReferencesAsync(
            ICharacterRepository characterRepository,
            Entities.Characters.Equipment.OwnershipHistory history,
            CancellationToken cancellationToken)
        {
            if (history.PreviousOwnerId is int previousOwnerId
                && !await characterRepository.ExistsInWorldAsync(previousOwnerId, history.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"Character with ID {previousOwnerId} does not exist in this world.");
            }

            if (history.NewOwnerId is int newOwnerId
                && !await characterRepository.ExistsInWorldAsync(newOwnerId, history.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"Character with ID {newOwnerId} does not exist in this world.");
            }
        }
    }
}
