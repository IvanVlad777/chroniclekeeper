using AutoMapper;
using ChronicleKeeper.Core.CQRS.AbilityLevels.Commands;
using ChronicleKeeper.Core.CQRS.AbilityLevels.Queries;
using ChronicleKeeper.Core.DTOs.AbilityLevel;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.AbilityLevels.Handlers
{
    public class GetAllAbilityLevelsQueryHandler : IRequestHandler<GetAllAbilityLevelsQuery, List<AbilityLevelDto>>
    {
        private readonly IAbilityLevelRepository _repository;
        private readonly IMapper _mapper;

        public GetAllAbilityLevelsQueryHandler(IAbilityLevelRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<AbilityLevelDto>> Handle(GetAllAbilityLevelsQuery request, CancellationToken cancellationToken)
        {
            var levels = await _repository.GetAllAsync(request.WorldId, request.AbilityId, cancellationToken);
            return _mapper.Map<List<AbilityLevelDto>>(levels);
        }
    }

    public class GetAbilityLevelByIdQueryHandler : IRequestHandler<GetAbilityLevelByIdQuery, AbilityLevelDto?>
    {
        private readonly IAbilityLevelRepository _repository;
        private readonly IMapper _mapper;

        public GetAbilityLevelByIdQueryHandler(IAbilityLevelRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AbilityLevelDto?> Handle(GetAbilityLevelByIdQuery request, CancellationToken cancellationToken)
        {
            var level = await _repository.FindByIdAsync(request.Id, cancellationToken);
            return level == null ? null : _mapper.Map<AbilityLevelDto>(level);
        }
    }

    public class CreateAbilityLevelCommandHandler : IRequestHandler<CreateAbilityLevelCommand, AbilityLevelDto>
    {
        private readonly IAbilityLevelRepository _repository;
        private readonly IAbilityRepository _abilityRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateAbilityLevelCommandHandler> _logger;

        public CreateAbilityLevelCommandHandler(
            IAbilityLevelRepository repository,
            IAbilityRepository abilityRepository,
            IMapper mapper,
            ILogger<CreateAbilityLevelCommandHandler> logger)
        {
            _repository = repository;
            _abilityRepository = abilityRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AbilityLevelDto> Handle(CreateAbilityLevelCommand request, CancellationToken cancellationToken)
        {
            var dto = request.AbilityLevelCreateDto;
            _logger.LogInformation("Creating new ability level: {Name}", dto.Name);

            var ability = await _abilityRepository.FindByIdAsync(dto.AbilityId, cancellationToken)
                ?? throw new DomainValidationException($"Ability with ID {dto.AbilityId} does not exist.");

            var level = _mapper.Map<Entities.Characters.Abilities.AbilityLevel>(dto);
            level.WorldId = ability.WorldId; // svijet razine uvijek = svijet sposobnosti

            var created = await _repository.CreateAsync(level, cancellationToken);
            return _mapper.Map<AbilityLevelDto>(created);
        }
    }

    public class UpdateAbilityLevelCommandHandler : IRequestHandler<UpdateAbilityLevelCommand, AbilityLevelDto>
    {
        private readonly IAbilityLevelRepository _repository;
        private readonly IMapper _mapper;

        public UpdateAbilityLevelCommandHandler(IAbilityLevelRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<AbilityLevelDto> Handle(UpdateAbilityLevelCommand request, CancellationToken cancellationToken)
        {
            var level = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("AbilityLevel", request.Id);

            _mapper.Map(request.AbilityLevelUpdateDto, level);
            var updated = await _repository.UpdateAsync(level, cancellationToken);
            return _mapper.Map<AbilityLevelDto>(updated);
        }
    }

    public class DeleteAbilityLevelCommandHandler : IRequestHandler<DeleteAbilityLevelCommand, bool>
    {
        private readonly IAbilityLevelRepository _repository;
        private readonly ILogger<DeleteAbilityLevelCommandHandler> _logger;

        public DeleteAbilityLevelCommandHandler(IAbilityLevelRepository repository, ILogger<DeleteAbilityLevelCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteAbilityLevelCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting ability level with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
