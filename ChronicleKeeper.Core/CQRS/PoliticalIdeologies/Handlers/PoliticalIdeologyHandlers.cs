using AutoMapper;
using ChronicleKeeper.Core.CQRS.PoliticalIdeologies.Commands;
using ChronicleKeeper.Core.CQRS.PoliticalIdeologies.Queries;
using ChronicleKeeper.Core.DTOs.PoliticalIdeology;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.PoliticalIdeologies.Handlers
{
    public class GetAllPoliticalIdeologiesQueryHandler : IRequestHandler<GetAllPoliticalIdeologiesQuery, List<PoliticalIdeologyDto>>
    {
        private readonly IPoliticalIdeologyRepository _repository;
        private readonly IMapper _mapper;

        public GetAllPoliticalIdeologiesQueryHandler(IPoliticalIdeologyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PoliticalIdeologyDto>> Handle(GetAllPoliticalIdeologiesQuery request, CancellationToken cancellationToken)
        {
            var ideologies = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<PoliticalIdeologyDto>>(ideologies);
        }
    }

    public class GetPoliticalIdeologyByIdQueryHandler : IRequestHandler<GetPoliticalIdeologyByIdQuery, PoliticalIdeologyDetailsDto?>
    {
        private readonly IPoliticalIdeologyRepository _repository;
        private readonly IMapper _mapper;

        public GetPoliticalIdeologyByIdQueryHandler(IPoliticalIdeologyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PoliticalIdeologyDetailsDto?> Handle(GetPoliticalIdeologyByIdQuery request, CancellationToken cancellationToken)
        {
            var ideology = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return ideology == null ? null : _mapper.Map<PoliticalIdeologyDetailsDto>(ideology);
        }
    }

    public class CreatePoliticalIdeologyCommandHandler : IRequestHandler<CreatePoliticalIdeologyCommand, PoliticalIdeologyDto>
    {
        private readonly IPoliticalIdeologyRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreatePoliticalIdeologyCommandHandler> _logger;

        public CreatePoliticalIdeologyCommandHandler(
            IPoliticalIdeologyRepository repository,
            IWorldRepository worldRepository,
            IMapper mapper,
            ILogger<CreatePoliticalIdeologyCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PoliticalIdeologyDto> Handle(CreatePoliticalIdeologyCommand request, CancellationToken cancellationToken)
        {
            var dto = request.PoliticalIdeologyCreateDto;
            _logger.LogInformation("Creating new political ideology: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Social.Politics.PoliticalIdeology>(dto), cancellationToken);
            return _mapper.Map<PoliticalIdeologyDto>(created);
        }
    }

    public class UpdatePoliticalIdeologyCommandHandler : IRequestHandler<UpdatePoliticalIdeologyCommand, PoliticalIdeologyDto>
    {
        private readonly IPoliticalIdeologyRepository _repository;
        private readonly IMapper _mapper;

        public UpdatePoliticalIdeologyCommandHandler(IPoliticalIdeologyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PoliticalIdeologyDto> Handle(UpdatePoliticalIdeologyCommand request, CancellationToken cancellationToken)
        {
            var ideology = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("PoliticalIdeology", request.Id);

            _mapper.Map(request.PoliticalIdeologyUpdateDto, ideology);
            var updated = await _repository.UpdateAsync(ideology, cancellationToken);
            return _mapper.Map<PoliticalIdeologyDto>(updated);
        }
    }

    public class DeletePoliticalIdeologyCommandHandler : IRequestHandler<DeletePoliticalIdeologyCommand, bool>
    {
        private readonly IPoliticalIdeologyRepository _repository;
        private readonly ILogger<DeletePoliticalIdeologyCommandHandler> _logger;

        public DeletePoliticalIdeologyCommandHandler(IPoliticalIdeologyRepository repository, ILogger<DeletePoliticalIdeologyCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeletePoliticalIdeologyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting political ideology with ID {Id}", request.Id);

            var partiesInUse = await _repository.CountPoliticalPartiesUsingIdeologyAsync(request.Id, cancellationToken);
            if (partiesInUse > 0)
            {
                throw new DomainValidationException(
                    $"This ideology is used by {partiesInUse} political party(ies). Reassign them first.");
            }

            var systemsInUse = await _repository.CountGovernmentSystemsUsingIdeologyAsync(request.Id, cancellationToken);
            if (systemsInUse > 0)
            {
                throw new DomainValidationException(
                    $"This ideology is used by {systemsInUse} government system(s). Reassign them first.");
            }

            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
