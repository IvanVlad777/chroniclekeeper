using AutoMapper;
using ChronicleKeeper.Core.CQRS.GovernmentSystems.Commands;
using ChronicleKeeper.Core.CQRS.GovernmentSystems.Queries;
using ChronicleKeeper.Core.DTOs.GovernmentSystem;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.GovernmentSystems.Handlers
{
    public class GetAllGovernmentSystemsQueryHandler : IRequestHandler<GetAllGovernmentSystemsQuery, List<GovernmentSystemDto>>
    {
        private readonly IGovernmentSystemRepository _repository;
        private readonly IMapper _mapper;

        public GetAllGovernmentSystemsQueryHandler(IGovernmentSystemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<GovernmentSystemDto>> Handle(GetAllGovernmentSystemsQuery request, CancellationToken cancellationToken)
        {
            var systems = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<GovernmentSystemDto>>(systems);
        }
    }

    public class GetGovernmentSystemByIdQueryHandler : IRequestHandler<GetGovernmentSystemByIdQuery, GovernmentSystemDetailsDto?>
    {
        private readonly IGovernmentSystemRepository _repository;
        private readonly IMapper _mapper;

        public GetGovernmentSystemByIdQueryHandler(IGovernmentSystemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GovernmentSystemDetailsDto?> Handle(GetGovernmentSystemByIdQuery request, CancellationToken cancellationToken)
        {
            var system = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return system == null ? null : _mapper.Map<GovernmentSystemDetailsDto>(system);
        }
    }

    public class CreateGovernmentSystemCommandHandler : IRequestHandler<CreateGovernmentSystemCommand, GovernmentSystemDto>
    {
        private readonly IGovernmentSystemRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IPoliticalIdeologyRepository _ideologyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateGovernmentSystemCommandHandler> _logger;

        public CreateGovernmentSystemCommandHandler(
            IGovernmentSystemRepository repository,
            IWorldRepository worldRepository,
            IPoliticalIdeologyRepository ideologyRepository,
            IMapper mapper,
            ILogger<CreateGovernmentSystemCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _ideologyRepository = ideologyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GovernmentSystemDto> Handle(CreateGovernmentSystemCommand request, CancellationToken cancellationToken)
        {
            var dto = request.GovernmentSystemCreateDto;
            _logger.LogInformation("Creating new government system: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            if (dto.PoliticalIdeologyId is int ideologyId)
            {
                var ideology = await _ideologyRepository.FindByIdAsync(ideologyId, cancellationToken)
                    ?? throw new DomainValidationException($"Political ideology with ID {ideologyId} does not exist.");
                if (ideology.WorldId != dto.WorldId)
                {
                    throw new DomainValidationException($"Political ideology with ID {ideologyId} does not belong to this world.");
                }
            }

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Social.Politics.GovernmentSystem>(dto), cancellationToken);
            return _mapper.Map<GovernmentSystemDto>(created);
        }
    }

    public class UpdateGovernmentSystemCommandHandler : IRequestHandler<UpdateGovernmentSystemCommand, GovernmentSystemDto>
    {
        private readonly IGovernmentSystemRepository _repository;
        private readonly IPoliticalIdeologyRepository _ideologyRepository;
        private readonly IMapper _mapper;

        public UpdateGovernmentSystemCommandHandler(
            IGovernmentSystemRepository repository,
            IPoliticalIdeologyRepository ideologyRepository,
            IMapper mapper)
        {
            _repository = repository;
            _ideologyRepository = ideologyRepository;
            _mapper = mapper;
        }

        public async Task<GovernmentSystemDto> Handle(UpdateGovernmentSystemCommand request, CancellationToken cancellationToken)
        {
            var system = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("GovernmentSystem", request.Id);

            var dto = request.GovernmentSystemUpdateDto;

            if (dto.PoliticalIdeologyId is int ideologyId)
            {
                var ideology = await _ideologyRepository.FindByIdAsync(ideologyId, cancellationToken)
                    ?? throw new DomainValidationException($"Political ideology with ID {ideologyId} does not exist.");
                if (ideology.WorldId != system.WorldId)
                {
                    throw new DomainValidationException($"Political ideology with ID {ideologyId} does not belong to this world.");
                }
            }

            _mapper.Map(dto, system);
            var updated = await _repository.UpdateAsync(system, cancellationToken);
            return _mapper.Map<GovernmentSystemDto>(updated);
        }
    }

    public class DeleteGovernmentSystemCommandHandler : IRequestHandler<DeleteGovernmentSystemCommand, bool>
    {
        private readonly IGovernmentSystemRepository _repository;
        private readonly ILogger<DeleteGovernmentSystemCommandHandler> _logger;

        public DeleteGovernmentSystemCommandHandler(IGovernmentSystemRepository repository, ILogger<DeleteGovernmentSystemCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteGovernmentSystemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting government system with ID {Id}", request.Id);

            var partiesInUse = await _repository.CountPoliticalPartiesUsingGovernmentSystemAsync(request.Id, cancellationToken);
            if (partiesInUse > 0)
            {
                throw new DomainValidationException(
                    $"This government system is used by {partiesInUse} political party(ies). Reassign them first.");
            }

            var locationsInUse = await _repository.CountLocationsUsingGovernmentSystemAsync(request.Id, cancellationToken);
            if (locationsInUse > 0)
            {
                throw new DomainValidationException(
                    $"This government system is used by {locationsInUse} location(s). Reassign them first.");
            }

            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
