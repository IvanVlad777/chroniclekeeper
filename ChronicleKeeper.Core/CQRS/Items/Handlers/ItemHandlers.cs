using AutoMapper;
using ChronicleKeeper.Core.CQRS.Items.Commands;
using ChronicleKeeper.Core.CQRS.Items.Queries;
using ChronicleKeeper.Core.DTOs.Item;
using ChronicleKeeper.Core.Entities.Characters.Equipment;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Items.Handlers
{
    public class GetAllItemsQueryHandler : IRequestHandler<GetAllItemsQuery, List<ItemDto>>
    {
        private readonly IItemRepository _repository;
        private readonly IMapper _mapper;

        public GetAllItemsQueryHandler(IItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ItemDto>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetAllAsync(request.WorldId, request.CurrentOwnerId, request.FactionId, cancellationToken);
            return _mapper.Map<List<ItemDto>>(items);
        }
    }

    public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ItemDetailsDto?>
    {
        private readonly IItemRepository _repository;
        private readonly IMapper _mapper;

        public GetItemByIdQueryHandler(IItemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ItemDetailsDto?> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return item == null ? null : _mapper.Map<ItemDetailsDto>(item);
        }
    }

    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, ItemDto>
    {
        private readonly IItemRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly ICharacterRepository _characterRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IFactionRepository _factionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateItemCommandHandler> _logger;

        public CreateItemCommandHandler(
            IItemRepository repository,
            IWorldRepository worldRepository,
            ICharacterRepository characterRepository,
            ILocationRepository locationRepository,
            IFactionRepository factionRepository,
            IMapper mapper,
            ILogger<CreateItemCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _characterRepository = characterRepository;
            _locationRepository = locationRepository;
            _factionRepository = factionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ItemDto> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var dto = request.ItemCreateDto;
            _logger.LogInformation("Creating new item: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            var item = _mapper.Map<Item>(dto);
            await ItemValidation.ValidateReferencesAsync(_characterRepository, _locationRepository, _factionRepository, item, cancellationToken);

            var created = await _repository.CreateAsync(item, cancellationToken);
            return _mapper.Map<ItemDto>(created);
        }
    }

    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, ItemDto>
    {
        private readonly IItemRepository _repository;
        private readonly ICharacterRepository _characterRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IFactionRepository _factionRepository;
        private readonly IMapper _mapper;

        public UpdateItemCommandHandler(
            IItemRepository repository,
            ICharacterRepository characterRepository,
            ILocationRepository locationRepository,
            IFactionRepository factionRepository,
            IMapper mapper)
        {
            _repository = repository;
            _characterRepository = characterRepository;
            _locationRepository = locationRepository;
            _factionRepository = factionRepository;
            _mapper = mapper;
        }

        public async Task<ItemDto> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Item", request.Id);

            _mapper.Map(request.ItemUpdateDto, item);
            await ItemValidation.ValidateReferencesAsync(_characterRepository, _locationRepository, _factionRepository, item, cancellationToken);

            var updated = await _repository.UpdateAsync(item, cancellationToken);
            return _mapper.Map<ItemDto>(updated);
        }
    }

    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, bool>
    {
        private readonly IItemRepository _repository;
        private readonly ILogger<DeleteItemCommandHandler> _logger;

        public DeleteItemCommandHandler(IItemRepository repository, ILogger<DeleteItemCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting item with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    internal static class ItemValidation
    {
        /// <summary>
        /// Trenutni vlasnik, lokacija skladištenja i frakcija moraju postojati U ISTOM SVIJETU kao predmet —
        /// cross-world reference bi kasnije blokirale brisanje tuđeg svijeta.
        /// </summary>
        public static async Task ValidateReferencesAsync(
            ICharacterRepository characterRepository,
            ILocationRepository locationRepository,
            IFactionRepository factionRepository,
            Item item,
            CancellationToken cancellationToken)
        {
            if (item.CurrentOwnerId is int ownerId
                && !await characterRepository.ExistsInWorldAsync(ownerId, item.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"Character with ID {ownerId} does not exist in this world.");
            }

            if (item.StoredAtId is int locationId
                && !await locationRepository.ExistsInWorldAsync(locationId, item.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"Location with ID {locationId} does not exist in this world.");
            }

            if (item.FactionId is int factionId)
            {
                var faction = await factionRepository.FindByIdAsync(factionId, cancellationToken);
                if (faction == null || faction.WorldId != item.WorldId)
                {
                    throw new DomainValidationException($"Faction with ID {factionId} does not exist in this world.");
                }
            }
        }
    }
}
