using AutoMapper;
using ChronicleKeeper.Core.CQRS.LegalSystems.Commands;
using ChronicleKeeper.Core.CQRS.LegalSystems.Queries;
using ChronicleKeeper.Core.DTOs.LegalSystem;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.LegalSystems.Handlers
{
    public class GetAllLegalSystemsQueryHandler : IRequestHandler<GetAllLegalSystemsQuery, List<LegalSystemDto>>
    {
        private readonly ILegalSystemRepository _repository;
        private readonly IMapper _mapper;

        public GetAllLegalSystemsQueryHandler(ILegalSystemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<LegalSystemDto>> Handle(GetAllLegalSystemsQuery request, CancellationToken cancellationToken)
        {
            var legalSystems = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<LegalSystemDto>>(legalSystems);
        }
    }

    public class GetLegalSystemByIdQueryHandler : IRequestHandler<GetLegalSystemByIdQuery, LegalSystemDetailsDto?>
    {
        private readonly ILegalSystemRepository _repository;
        private readonly IMapper _mapper;

        public GetLegalSystemByIdQueryHandler(ILegalSystemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<LegalSystemDetailsDto?> Handle(GetLegalSystemByIdQuery request, CancellationToken cancellationToken)
        {
            var legalSystem = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return legalSystem == null ? null : _mapper.Map<LegalSystemDetailsDto>(legalSystem);
        }
    }

    public class CreateLegalSystemCommandHandler : IRequestHandler<CreateLegalSystemCommand, LegalSystemDto>
    {
        private readonly ILegalSystemRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateLegalSystemCommandHandler> _logger;

        public CreateLegalSystemCommandHandler(
            ILegalSystemRepository repository,
            IWorldRepository worldRepository,
            IMapper mapper,
            ILogger<CreateLegalSystemCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LegalSystemDto> Handle(CreateLegalSystemCommand request, CancellationToken cancellationToken)
        {
            var dto = request.LegalSystemCreateDto;
            _logger.LogInformation("Creating new legal system: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Social.Politics.LegalSystem>(dto), cancellationToken);
            return _mapper.Map<LegalSystemDto>(created);
        }
    }

    public class UpdateLegalSystemCommandHandler : IRequestHandler<UpdateLegalSystemCommand, LegalSystemDto>
    {
        private readonly ILegalSystemRepository _repository;
        private readonly IMapper _mapper;

        public UpdateLegalSystemCommandHandler(ILegalSystemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<LegalSystemDto> Handle(UpdateLegalSystemCommand request, CancellationToken cancellationToken)
        {
            var legalSystem = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("LegalSystem", request.Id);

            _mapper.Map(request.LegalSystemUpdateDto, legalSystem);
            var updated = await _repository.UpdateAsync(legalSystem, cancellationToken);
            return _mapper.Map<LegalSystemDto>(updated);
        }
    }

    public class DeleteLegalSystemCommandHandler : IRequestHandler<DeleteLegalSystemCommand, bool>
    {
        private readonly ILegalSystemRepository _repository;
        private readonly ILogger<DeleteLegalSystemCommandHandler> _logger;

        public DeleteLegalSystemCommandHandler(ILegalSystemRepository repository, ILogger<DeleteLegalSystemCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteLegalSystemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting legal system with ID {Id}", request.Id);

            var locationsInUse = await _repository.CountLocationsUsingLegalSystemAsync(request.Id, cancellationToken);
            if (locationsInUse > 0)
            {
                throw new DomainValidationException(
                    $"This legal system is used by {locationsInUse} location(s). Reassign them first.");
            }

            var guildsInUse = await _repository.CountGuildsUsingLegalSystemAsync(request.Id, cancellationToken);
            if (guildsInUse > 0)
            {
                throw new DomainValidationException(
                    $"This legal system is used by {guildsInUse} guild(s). Reassign them first.");
            }

            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
