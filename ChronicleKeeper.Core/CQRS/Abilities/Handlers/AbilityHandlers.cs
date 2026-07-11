using AutoMapper;
using ChronicleKeeper.Core.CQRS.Abilities.Commands;
using ChronicleKeeper.Core.CQRS.Abilities.Queries;
using ChronicleKeeper.Core.DTOs.Ability;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Abilities.Handlers
{
    public class GetAllAbilitiesQueryHandler : IRequestHandler<GetAllAbilitiesQuery, List<AbilityDto>>
    {
        private readonly IAbilityRepository _repository;
        private readonly IMapper _mapper;

        public GetAllAbilitiesQueryHandler(IAbilityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<AbilityDto>> Handle(GetAllAbilitiesQuery request, CancellationToken cancellationToken)
        {
            var abilities = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<AbilityDto>>(abilities);
        }
    }

    public class GetAbilityByIdQueryHandler : IRequestHandler<GetAbilityByIdQuery, AbilityDto?>
    {
        private readonly IAbilityRepository _repository;
        private readonly IMapper _mapper;

        public GetAbilityByIdQueryHandler(IAbilityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AbilityDto?> Handle(GetAbilityByIdQuery request, CancellationToken cancellationToken)
        {
            var ability = await _repository.FindByIdAsync(request.Id, cancellationToken);
            return ability == null ? null : _mapper.Map<AbilityDto>(ability);
        }
    }

    public class CreateAbilityCommandHandler : IRequestHandler<CreateAbilityCommand, AbilityDto>
    {
        private readonly IAbilityRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateAbilityCommandHandler> _logger;

        public CreateAbilityCommandHandler(
            IAbilityRepository repository,
            IWorldRepository worldRepository,
            IMapper mapper,
            ILogger<CreateAbilityCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AbilityDto> Handle(CreateAbilityCommand request, CancellationToken cancellationToken)
        {
            var dto = request.AbilityCreateDto;
            _logger.LogInformation("Creating new ability: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            var ability = _mapper.Map<Entities.Characters.Abilities.Ability>(dto);
            var created = await _repository.CreateAsync(ability, cancellationToken);
            return _mapper.Map<AbilityDto>(created);
        }
    }

    public class UpdateAbilityCommandHandler : IRequestHandler<UpdateAbilityCommand, AbilityDto>
    {
        private readonly IAbilityRepository _repository;
        private readonly IMapper _mapper;

        public UpdateAbilityCommandHandler(IAbilityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AbilityDto> Handle(UpdateAbilityCommand request, CancellationToken cancellationToken)
        {
            var ability = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Ability", request.Id);

            _mapper.Map(request.AbilityUpdateDto, ability);
            var updated = await _repository.UpdateAsync(ability, cancellationToken);
            return _mapper.Map<AbilityDto>(updated);
        }
    }

    public class DeleteAbilityCommandHandler : IRequestHandler<DeleteAbilityCommand, bool>
    {
        private readonly IAbilityRepository _repository;
        private readonly ILogger<DeleteAbilityCommandHandler> _logger;

        public DeleteAbilityCommandHandler(IAbilityRepository repository, ILogger<DeleteAbilityCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteAbilityCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting ability with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
