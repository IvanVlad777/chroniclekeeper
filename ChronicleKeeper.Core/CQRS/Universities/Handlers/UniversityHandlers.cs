using AutoMapper;
using ChronicleKeeper.Core.CQRS.Universities.Commands;
using ChronicleKeeper.Core.CQRS.Universities.Queries;
using ChronicleKeeper.Core.DTOs.University;
using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Universities.Handlers
{
    public class GetAllUniversitiesQueryHandler : IRequestHandler<GetAllUniversitiesQuery, List<UniversityDto>>
    {
        private readonly IUniversityRepository _repository;
        private readonly IMapper _mapper;

        public GetAllUniversitiesQueryHandler(IUniversityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<UniversityDto>> Handle(GetAllUniversitiesQuery request, CancellationToken cancellationToken)
        {
            var universities = await _repository.GetAllAsync(request.WorldId, request.EducationSystemId, cancellationToken);
            return _mapper.Map<List<UniversityDto>>(universities);
        }
    }

    public class GetUniversityByIdQueryHandler : IRequestHandler<GetUniversityByIdQuery, UniversityDetailsDto?>
    {
        private readonly IUniversityRepository _repository;
        private readonly IMapper _mapper;

        public GetUniversityByIdQueryHandler(IUniversityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UniversityDetailsDto?> Handle(GetUniversityByIdQuery request, CancellationToken cancellationToken)
        {
            var university = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return university == null ? null : _mapper.Map<UniversityDetailsDto>(university);
        }
    }

    public class CreateUniversityCommandHandler : IRequestHandler<CreateUniversityCommand, UniversityDto>
    {
        private readonly IUniversityRepository _repository;
        private readonly IEducationSystemRepository _educationSystemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateUniversityCommandHandler> _logger;

        public CreateUniversityCommandHandler(
            IUniversityRepository repository,
            IEducationSystemRepository educationSystemRepository,
            IMapper mapper,
            ILogger<CreateUniversityCommandHandler> logger)
        {
            _repository = repository;
            _educationSystemRepository = educationSystemRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UniversityDto> Handle(CreateUniversityCommand request, CancellationToken cancellationToken)
        {
            var dto = request.UniversityCreateDto;
            _logger.LogInformation("Creating new university: {Name}", dto.Name);

            var educationSystem = await _educationSystemRepository.FindByIdAsync(dto.EducationSystemId, cancellationToken)
                ?? throw new DomainValidationException($"Education system with ID {dto.EducationSystemId} does not exist.");

            var university = _mapper.Map<University>(dto);
            university.WorldId = educationSystem.WorldId; // svijet sveučilišta uvijek = svijet sustava obrazovanja

            var created = await _repository.CreateAsync(university, cancellationToken);
            return _mapper.Map<UniversityDto>(created);
        }
    }

    public class UpdateUniversityCommandHandler : IRequestHandler<UpdateUniversityCommand, UniversityDto>
    {
        private readonly IUniversityRepository _repository;
        private readonly IMapper _mapper;

        public UpdateUniversityCommandHandler(IUniversityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UniversityDto> Handle(UpdateUniversityCommand request, CancellationToken cancellationToken)
        {
            var university = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("University", request.Id);

            _mapper.Map(request.UniversityUpdateDto, university);
            var updated = await _repository.UpdateAsync(university, cancellationToken);
            return _mapper.Map<UniversityDto>(updated);
        }
    }

    public class DeleteUniversityCommandHandler : IRequestHandler<DeleteUniversityCommand, bool>
    {
        private readonly IUniversityRepository _repository;
        private readonly ILogger<DeleteUniversityCommandHandler> _logger;

        public DeleteUniversityCommandHandler(IUniversityRepository repository, ILogger<DeleteUniversityCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteUniversityCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting university with ID {Id}", request.Id);

            var inUseByRecords = await _repository.CountEducationRecordsUsingUniversityAsync(request.Id, cancellationToken);
            if (inUseByRecords > 0)
            {
                throw new DomainValidationException(
                    $"This university is used by {inUseByRecords} education record(s). Remove them first.");
            }

            var inUseByLibraries = await _repository.CountLibrariesUsingUniversityAsync(request.Id, cancellationToken);
            if (inUseByLibraries > 0)
            {
                throw new DomainValidationException(
                    $"This university is referenced by {inUseByLibraries} librar{(inUseByLibraries == 1 ? "y" : "ies")}. Reassign them first.");
            }

            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    public class AddUniversityStudentCommandHandler : IRequestHandler<AddUniversityStudentCommand, bool>
    {
        private readonly IUniversityRepository _repository;
        private readonly ICharacterRepository _characterRepository;

        public AddUniversityStudentCommandHandler(IUniversityRepository repository, ICharacterRepository characterRepository)
        {
            _repository = repository;
            _characterRepository = characterRepository;
        }

        public async Task<bool> Handle(AddUniversityStudentCommand request, CancellationToken cancellationToken)
        {
            var university = await _repository.FindByIdAsync(request.UniversityId, cancellationToken)
                ?? throw new EntityNotFoundException("University", request.UniversityId);
            if (!await _characterRepository.ExistsInWorldAsync(request.CharacterId, university.WorldId, cancellationToken))
                throw new DomainValidationException($"Character with ID {request.CharacterId} does not exist in this world.");
            if (await _repository.IsStudentLinkedAsync(request.UniversityId, request.CharacterId, cancellationToken))
                throw new DomainValidationException("This character is already a student of the university.");

            await _repository.AddStudentAsync(request.UniversityId, request.CharacterId, cancellationToken);
            return true;
        }
    }

    public class RemoveUniversityStudentCommandHandler : IRequestHandler<RemoveUniversityStudentCommand, bool>
    {
        private readonly IUniversityRepository _repository;
        public RemoveUniversityStudentCommandHandler(IUniversityRepository repository) => _repository = repository;
        public Task<bool> Handle(RemoveUniversityStudentCommand request, CancellationToken cancellationToken)
            => _repository.RemoveStudentAsync(request.UniversityId, request.CharacterId, cancellationToken);
    }

    public class AddUniversityProfessorCommandHandler : IRequestHandler<AddUniversityProfessorCommand, bool>
    {
        private readonly IUniversityRepository _repository;
        private readonly ICharacterRepository _characterRepository;

        public AddUniversityProfessorCommandHandler(IUniversityRepository repository, ICharacterRepository characterRepository)
        {
            _repository = repository;
            _characterRepository = characterRepository;
        }

        public async Task<bool> Handle(AddUniversityProfessorCommand request, CancellationToken cancellationToken)
        {
            var university = await _repository.FindByIdAsync(request.UniversityId, cancellationToken)
                ?? throw new EntityNotFoundException("University", request.UniversityId);
            if (!await _characterRepository.ExistsInWorldAsync(request.CharacterId, university.WorldId, cancellationToken))
                throw new DomainValidationException($"Character with ID {request.CharacterId} does not exist in this world.");
            if (await _repository.IsProfessorLinkedAsync(request.UniversityId, request.CharacterId, cancellationToken))
                throw new DomainValidationException("This character is already a professor of the university.");

            await _repository.AddProfessorAsync(request.UniversityId, request.CharacterId, cancellationToken);
            return true;
        }
    }

    public class RemoveUniversityProfessorCommandHandler : IRequestHandler<RemoveUniversityProfessorCommand, bool>
    {
        private readonly IUniversityRepository _repository;
        public RemoveUniversityProfessorCommandHandler(IUniversityRepository repository) => _repository = repository;
        public Task<bool> Handle(RemoveUniversityProfessorCommand request, CancellationToken cancellationToken)
            => _repository.RemoveProfessorAsync(request.UniversityId, request.CharacterId, cancellationToken);
    }
}
