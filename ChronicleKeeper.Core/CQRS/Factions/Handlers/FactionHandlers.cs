using AutoMapper;
using ChronicleKeeper.Core.CQRS.Factions.Commands;
using ChronicleKeeper.Core.CQRS.Factions.Queries;
using ChronicleKeeper.Core.DTOs.Faction;
using ChronicleKeeper.Core.Entities.Social;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Factions.Handlers
{
    public class GetAllFactionsQueryHandler : IRequestHandler<GetAllFactionsQuery, List<FactionDto>>
    {
        private readonly IFactionRepository _repository;
        private readonly IMapper _mapper;

        public GetAllFactionsQueryHandler(IFactionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<FactionDto>> Handle(GetAllFactionsQuery request, CancellationToken cancellationToken)
        {
            var factions = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<FactionDto>>(factions);
        }
    }

    public class GetFactionByIdQueryHandler : IRequestHandler<GetFactionByIdQuery, FactionDetailsDto?>
    {
        private readonly IFactionRepository _repository;
        private readonly IMapper _mapper;

        public GetFactionByIdQueryHandler(IFactionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<FactionDetailsDto?> Handle(GetFactionByIdQuery request, CancellationToken cancellationToken)
        {
            var faction = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return faction == null ? null : _mapper.Map<FactionDetailsDto>(faction);
        }
    }

    public class CreateFactionCommandHandler : IRequestHandler<CreateFactionCommand, FactionDto>
    {
        private readonly IFactionRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly ICharacterRepository _characterRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateFactionCommandHandler> _logger;

        public CreateFactionCommandHandler(
            IFactionRepository repository,
            IWorldRepository worldRepository,
            ICharacterRepository characterRepository,
            ILocationRepository locationRepository,
            IMapper mapper,
            ILogger<CreateFactionCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _characterRepository = characterRepository;
            _locationRepository = locationRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FactionDto> Handle(CreateFactionCommand request, CancellationToken cancellationToken)
        {
            var dto = request.FactionCreateDto;
            _logger.LogInformation("Creating new faction: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            var faction = _mapper.Map<Faction>(dto);
            await FactionValidation.ValidateReferencesAsync(_characterRepository, _locationRepository, faction, cancellationToken);

            var created = await _repository.CreateAsync(faction, cancellationToken);
            return _mapper.Map<FactionDto>(created);
        }
    }

    public class UpdateFactionCommandHandler : IRequestHandler<UpdateFactionCommand, FactionDto>
    {
        private readonly IFactionRepository _repository;
        private readonly ICharacterRepository _characterRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public UpdateFactionCommandHandler(
            IFactionRepository repository,
            ICharacterRepository characterRepository,
            ILocationRepository locationRepository,
            IMapper mapper)
        {
            _repository = repository;
            _characterRepository = characterRepository;
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public async Task<FactionDto> Handle(UpdateFactionCommand request, CancellationToken cancellationToken)
        {
            var faction = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Faction", request.Id);

            _mapper.Map(request.FactionUpdateDto, faction);
            await FactionValidation.ValidateReferencesAsync(_characterRepository, _locationRepository, faction, cancellationToken);

            var updated = await _repository.UpdateAsync(faction, cancellationToken);
            return _mapper.Map<FactionDto>(updated);
        }
    }

    public class DeleteFactionCommandHandler : IRequestHandler<DeleteFactionCommand, bool>
    {
        private readonly IFactionRepository _repository;
        private readonly ILogger<DeleteFactionCommandHandler> _logger;

        public DeleteFactionCommandHandler(IFactionRepository repository, ILogger<DeleteFactionCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteFactionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting faction with ID {Id}", request.Id);
            // DB kaskadira FactionMembers i FactionTags
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    public class AddFactionMemberCommandHandler : IRequestHandler<AddFactionMemberCommand, FactionMemberDto>
    {
        private readonly IFactionRepository _repository;
        private readonly ICharacterRepository _characterRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddFactionMemberCommandHandler> _logger;

        public AddFactionMemberCommandHandler(
            IFactionRepository repository,
            ICharacterRepository characterRepository,
            IMapper mapper,
            ILogger<AddFactionMemberCommandHandler> logger)
        {
            _repository = repository;
            _characterRepository = characterRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<FactionMemberDto> Handle(AddFactionMemberCommand request, CancellationToken cancellationToken)
        {
            var faction = await _repository.FindByIdAsync(request.FactionId, cancellationToken)
                ?? throw new EntityNotFoundException("Faction", request.FactionId);

            var dto = request.MemberDto;
            if (!await _characterRepository.ExistsInWorldAsync(dto.CharacterId, faction.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"Character with ID {dto.CharacterId} does not exist in this world.");
            }
            if (await _repository.IsMemberAsync(request.FactionId, dto.CharacterId, cancellationToken))
            {
                throw new DomainValidationException("This character is already a member of the faction.");
            }

            var member = new FactionMember
            {
                FactionId = request.FactionId,
                CharacterId = dto.CharacterId,
                Role = dto.Role,
                IsSecret = dto.IsSecret
            };
            var created = await _repository.AddMemberAsync(member, cancellationToken);

            _logger.LogInformation("Added character {CharacterId} to faction {FactionId}", dto.CharacterId, request.FactionId);
            return _mapper.Map<FactionMemberDto>(created);
        }
    }

    public class RemoveFactionMemberCommandHandler : IRequestHandler<RemoveFactionMemberCommand, bool>
    {
        private readonly IFactionRepository _repository;
        private readonly ILogger<RemoveFactionMemberCommandHandler> _logger;

        public RemoveFactionMemberCommandHandler(IFactionRepository repository, ILogger<RemoveFactionMemberCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(RemoveFactionMemberCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Removing character {CharacterId} from faction {FactionId}", request.CharacterId, request.FactionId);
            return await _repository.RemoveMemberAsync(request.FactionId, request.CharacterId, cancellationToken);
        }
    }

    internal static class FactionValidation
    {
        /// <summary>Leader i Headquarters moraju postojati u istom svijetu kao frakcija.</summary>
        public static async Task ValidateReferencesAsync(
            ICharacterRepository characterRepository,
            ILocationRepository locationRepository,
            Faction faction,
            CancellationToken cancellationToken)
        {
            if (faction.LeaderId is int leaderId
                && !await characterRepository.ExistsInWorldAsync(leaderId, faction.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"Leader character with ID {leaderId} does not exist in this world.");
            }

            if (faction.HeadquartersId is int hqId
                && !await locationRepository.ExistsInWorldAsync(hqId, faction.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"Headquarters location with ID {hqId} does not exist in this world.");
            }
        }
    }
}
