using AutoMapper;
using ChronicleKeeper.Core.CQRS.PoliticalParties.Commands;
using ChronicleKeeper.Core.CQRS.PoliticalParties.Queries;
using ChronicleKeeper.Core.DTOs.PoliticalParty;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.PoliticalParties.Handlers
{
    public class GetAllPoliticalPartiesQueryHandler : IRequestHandler<GetAllPoliticalPartiesQuery, List<PoliticalPartyDto>>
    {
        private readonly IPoliticalPartyRepository _repository;
        private readonly IMapper _mapper;

        public GetAllPoliticalPartiesQueryHandler(IPoliticalPartyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PoliticalPartyDto>> Handle(GetAllPoliticalPartiesQuery request, CancellationToken cancellationToken)
        {
            var parties = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<PoliticalPartyDto>>(parties);
        }
    }

    public class GetPoliticalPartyByIdQueryHandler : IRequestHandler<GetPoliticalPartyByIdQuery, PoliticalPartyDetailsDto?>
    {
        private readonly IPoliticalPartyRepository _repository;
        private readonly IMapper _mapper;

        public GetPoliticalPartyByIdQueryHandler(IPoliticalPartyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PoliticalPartyDetailsDto?> Handle(GetPoliticalPartyByIdQuery request, CancellationToken cancellationToken)
        {
            var party = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return party == null ? null : _mapper.Map<PoliticalPartyDetailsDto>(party);
        }
    }

    public class CreatePoliticalPartyCommandHandler : IRequestHandler<CreatePoliticalPartyCommand, PoliticalPartyDto>
    {
        private readonly IPoliticalPartyRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IPoliticalIdeologyRepository _ideologyRepository;
        private readonly IGovernmentSystemRepository _governmentSystemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreatePoliticalPartyCommandHandler> _logger;

        public CreatePoliticalPartyCommandHandler(
            IPoliticalPartyRepository repository,
            IWorldRepository worldRepository,
            IPoliticalIdeologyRepository ideologyRepository,
            IGovernmentSystemRepository governmentSystemRepository,
            IMapper mapper,
            ILogger<CreatePoliticalPartyCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _ideologyRepository = ideologyRepository;
            _governmentSystemRepository = governmentSystemRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PoliticalPartyDto> Handle(CreatePoliticalPartyCommand request, CancellationToken cancellationToken)
        {
            var dto = request.PoliticalPartyCreateDto;
            _logger.LogInformation("Creating new political party: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            var ideology = await _ideologyRepository.FindByIdAsync(dto.PoliticalIdeologyId, cancellationToken)
                ?? throw new DomainValidationException($"Political ideology with ID {dto.PoliticalIdeologyId} does not exist.");
            if (ideology.WorldId != dto.WorldId)
            {
                throw new DomainValidationException($"Political ideology with ID {dto.PoliticalIdeologyId} does not belong to this world.");
            }

            if (dto.GovernmentSystemId is int governmentSystemId)
            {
                var system = await _governmentSystemRepository.FindByIdAsync(governmentSystemId, cancellationToken)
                    ?? throw new DomainValidationException($"Government system with ID {governmentSystemId} does not exist.");
                if (system.WorldId != dto.WorldId)
                {
                    throw new DomainValidationException($"Government system with ID {governmentSystemId} does not belong to this world.");
                }
            }

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Social.Politics.PoliticalParty>(dto), cancellationToken);
            return _mapper.Map<PoliticalPartyDto>(created);
        }
    }

    public class UpdatePoliticalPartyCommandHandler : IRequestHandler<UpdatePoliticalPartyCommand, PoliticalPartyDto>
    {
        private readonly IPoliticalPartyRepository _repository;
        private readonly IPoliticalIdeologyRepository _ideologyRepository;
        private readonly IGovernmentSystemRepository _governmentSystemRepository;
        private readonly IMapper _mapper;

        public UpdatePoliticalPartyCommandHandler(
            IPoliticalPartyRepository repository,
            IPoliticalIdeologyRepository ideologyRepository,
            IGovernmentSystemRepository governmentSystemRepository,
            IMapper mapper)
        {
            _repository = repository;
            _ideologyRepository = ideologyRepository;
            _governmentSystemRepository = governmentSystemRepository;
            _mapper = mapper;
        }

        public async Task<PoliticalPartyDto> Handle(UpdatePoliticalPartyCommand request, CancellationToken cancellationToken)
        {
            var party = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("PoliticalParty", request.Id);

            var dto = request.PoliticalPartyUpdateDto;

            var ideology = await _ideologyRepository.FindByIdAsync(dto.PoliticalIdeologyId, cancellationToken)
                ?? throw new DomainValidationException($"Political ideology with ID {dto.PoliticalIdeologyId} does not exist.");
            if (ideology.WorldId != party.WorldId)
            {
                throw new DomainValidationException($"Political ideology with ID {dto.PoliticalIdeologyId} does not belong to this world.");
            }

            if (dto.GovernmentSystemId is int governmentSystemId)
            {
                var system = await _governmentSystemRepository.FindByIdAsync(governmentSystemId, cancellationToken)
                    ?? throw new DomainValidationException($"Government system with ID {governmentSystemId} does not exist.");
                if (system.WorldId != party.WorldId)
                {
                    throw new DomainValidationException($"Government system with ID {governmentSystemId} does not belong to this world.");
                }
            }

            _mapper.Map(dto, party);
            var updated = await _repository.UpdateAsync(party, cancellationToken);
            return _mapper.Map<PoliticalPartyDto>(updated);
        }
    }

    public class DeletePoliticalPartyCommandHandler : IRequestHandler<DeletePoliticalPartyCommand, bool>
    {
        private readonly IPoliticalPartyRepository _repository;
        private readonly ILogger<DeletePoliticalPartyCommandHandler> _logger;

        public DeletePoliticalPartyCommandHandler(IPoliticalPartyRepository repository, ILogger<DeletePoliticalPartyCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeletePoliticalPartyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting political party with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
