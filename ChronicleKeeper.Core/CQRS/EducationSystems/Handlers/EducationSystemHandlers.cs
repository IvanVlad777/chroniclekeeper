using AutoMapper;
using ChronicleKeeper.Core.CQRS.EducationSystems.Commands;
using ChronicleKeeper.Core.CQRS.EducationSystems.Queries;
using ChronicleKeeper.Core.DTOs.EducationSystem;
using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.EducationSystems.Handlers
{
    public class GetAllEducationSystemsQueryHandler : IRequestHandler<GetAllEducationSystemsQuery, List<EducationSystemDto>>
    {
        private readonly IEducationSystemRepository _repository;
        private readonly IMapper _mapper;

        public GetAllEducationSystemsQueryHandler(IEducationSystemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<EducationSystemDto>> Handle(GetAllEducationSystemsQuery request, CancellationToken cancellationToken)
        {
            var educationSystems = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<EducationSystemDto>>(educationSystems);
        }
    }

    public class GetEducationSystemByIdQueryHandler : IRequestHandler<GetEducationSystemByIdQuery, EducationSystemDetailsDto?>
    {
        private readonly IEducationSystemRepository _repository;
        private readonly IMapper _mapper;

        public GetEducationSystemByIdQueryHandler(IEducationSystemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EducationSystemDetailsDto?> Handle(GetEducationSystemByIdQuery request, CancellationToken cancellationToken)
        {
            var educationSystem = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return educationSystem == null ? null : _mapper.Map<EducationSystemDetailsDto>(educationSystem);
        }
    }

    public class CreateEducationSystemCommandHandler : IRequestHandler<CreateEducationSystemCommand, EducationSystemDto>
    {
        private readonly IEducationSystemRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateEducationSystemCommandHandler> _logger;

        public CreateEducationSystemCommandHandler(
            IEducationSystemRepository repository,
            IWorldRepository worldRepository,
            IMapper mapper,
            ILogger<CreateEducationSystemCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<EducationSystemDto> Handle(CreateEducationSystemCommand request, CancellationToken cancellationToken)
        {
            var dto = request.EducationSystemCreateDto;
            _logger.LogInformation("Creating new education system: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            var created = await _repository.CreateAsync(_mapper.Map<EducationSystem>(dto), cancellationToken);
            return _mapper.Map<EducationSystemDto>(created);
        }
    }

    public class UpdateEducationSystemCommandHandler : IRequestHandler<UpdateEducationSystemCommand, EducationSystemDto>
    {
        private readonly IEducationSystemRepository _repository;
        private readonly IMapper _mapper;

        public UpdateEducationSystemCommandHandler(IEducationSystemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EducationSystemDto> Handle(UpdateEducationSystemCommand request, CancellationToken cancellationToken)
        {
            var educationSystem = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("EducationSystem", request.Id);

            _mapper.Map(request.EducationSystemUpdateDto, educationSystem);
            var updated = await _repository.UpdateAsync(educationSystem, cancellationToken);
            return _mapper.Map<EducationSystemDto>(updated);
        }
    }

    public class DeleteEducationSystemCommandHandler : IRequestHandler<DeleteEducationSystemCommand, bool>
    {
        private readonly IEducationSystemRepository _repository;
        private readonly ILogger<DeleteEducationSystemCommandHandler> _logger;

        public DeleteEducationSystemCommandHandler(IEducationSystemRepository repository, ILogger<DeleteEducationSystemCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteEducationSystemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting education system with ID {Id}", request.Id);

            // No delete-guard needed: Schools and Universities cascade away with their EducationSystem
            // (compositional-owner relationship, same as Timeline -> TimelineEvent).
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
