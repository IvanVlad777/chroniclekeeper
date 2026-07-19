using AutoMapper;
using ChronicleKeeper.Core.CQRS.UniversityMajors.Commands;
using ChronicleKeeper.Core.CQRS.UniversityMajors.Queries;
using ChronicleKeeper.Core.DTOs.UniversityMajor;
using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.UniversityMajors.Handlers
{
    public class GetAllUniversityMajorsQueryHandler : IRequestHandler<GetAllUniversityMajorsQuery, List<UniversityMajorDto>>
    {
        private readonly IUniversityMajorRepository _repository;
        private readonly IMapper _mapper;

        public GetAllUniversityMajorsQueryHandler(IUniversityMajorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<UniversityMajorDto>> Handle(GetAllUniversityMajorsQuery request, CancellationToken cancellationToken)
        {
            var majors = await _repository.GetAllAsync(request.WorldId, request.UniversityId, cancellationToken);
            return _mapper.Map<List<UniversityMajorDto>>(majors);
        }
    }

    public class GetUniversityMajorByIdQueryHandler : IRequestHandler<GetUniversityMajorByIdQuery, UniversityMajorDetailsDto?>
    {
        private readonly IUniversityMajorRepository _repository;
        private readonly IMapper _mapper;

        public GetUniversityMajorByIdQueryHandler(IUniversityMajorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UniversityMajorDetailsDto?> Handle(GetUniversityMajorByIdQuery request, CancellationToken cancellationToken)
        {
            var major = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return major == null ? null : _mapper.Map<UniversityMajorDetailsDto>(major);
        }
    }

    public class AddUniversityMajorProfessorCommandHandler : IRequestHandler<AddUniversityMajorProfessorCommand, bool>
    {
        private readonly IUniversityMajorRepository _repository;
        private readonly ICharacterRepository _characterRepository;

        public AddUniversityMajorProfessorCommandHandler(IUniversityMajorRepository repository, ICharacterRepository characterRepository)
        {
            _repository = repository;
            _characterRepository = characterRepository;
        }

        public async Task<bool> Handle(AddUniversityMajorProfessorCommand request, CancellationToken cancellationToken)
        {
            var major = await _repository.FindByIdAsync(request.UniversityMajorId, cancellationToken)
                ?? throw new EntityNotFoundException("UniversityMajor", request.UniversityMajorId);
            if (!await _characterRepository.ExistsInWorldAsync(request.CharacterId, major.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"Character with ID {request.CharacterId} does not exist in this world.");
            }
            if (await _repository.IsProfessorLinkedAsync(request.UniversityMajorId, request.CharacterId, cancellationToken))
            {
                throw new DomainValidationException("This character is already a professor of the major.");
            }
            await _repository.AddProfessorAsync(request.UniversityMajorId, request.CharacterId, cancellationToken);
            return true;
        }
    }

    public class RemoveUniversityMajorProfessorCommandHandler : IRequestHandler<RemoveUniversityMajorProfessorCommand, bool>
    {
        private readonly IUniversityMajorRepository _repository;

        public RemoveUniversityMajorProfessorCommandHandler(IUniversityMajorRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(RemoveUniversityMajorProfessorCommand request, CancellationToken cancellationToken)
        {
            return _repository.RemoveProfessorAsync(request.UniversityMajorId, request.CharacterId, cancellationToken);
        }
    }

    public class AddUniversityMajorStudentCommandHandler : IRequestHandler<AddUniversityMajorStudentCommand, bool>
    {
        private readonly IUniversityMajorRepository _repository;
        private readonly ICharacterRepository _characterRepository;

        public AddUniversityMajorStudentCommandHandler(IUniversityMajorRepository repository, ICharacterRepository characterRepository)
        {
            _repository = repository;
            _characterRepository = characterRepository;
        }

        public async Task<bool> Handle(AddUniversityMajorStudentCommand request, CancellationToken cancellationToken)
        {
            var major = await _repository.FindByIdAsync(request.UniversityMajorId, cancellationToken)
                ?? throw new EntityNotFoundException("UniversityMajor", request.UniversityMajorId);
            if (!await _characterRepository.ExistsInWorldAsync(request.CharacterId, major.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"Character with ID {request.CharacterId} does not exist in this world.");
            }
            if (await _repository.IsStudentLinkedAsync(request.UniversityMajorId, request.CharacterId, cancellationToken))
            {
                throw new DomainValidationException("This character is already a student of the major.");
            }
            await _repository.AddStudentAsync(request.UniversityMajorId, request.CharacterId, cancellationToken);
            return true;
        }
    }

    public class RemoveUniversityMajorStudentCommandHandler : IRequestHandler<RemoveUniversityMajorStudentCommand, bool>
    {
        private readonly IUniversityMajorRepository _repository;

        public RemoveUniversityMajorStudentCommandHandler(IUniversityMajorRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(RemoveUniversityMajorStudentCommand request, CancellationToken cancellationToken)
        {
            return _repository.RemoveStudentAsync(request.UniversityMajorId, request.CharacterId, cancellationToken);
        }
    }

    public class CreateUniversityMajorCommandHandler : IRequestHandler<CreateUniversityMajorCommand, UniversityMajorDto>
    {
        private readonly IUniversityMajorRepository _repository;
        private readonly IUniversityRepository _universityRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateUniversityMajorCommandHandler> _logger;

        public CreateUniversityMajorCommandHandler(
            IUniversityMajorRepository repository,
            IUniversityRepository universityRepository,
            IMapper mapper,
            ILogger<CreateUniversityMajorCommandHandler> logger)
        {
            _repository = repository;
            _universityRepository = universityRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UniversityMajorDto> Handle(CreateUniversityMajorCommand request, CancellationToken cancellationToken)
        {
            var dto = request.UniversityMajorCreateDto;
            _logger.LogInformation("Creating new university major: {Name}", dto.Name);

            var university = await _universityRepository.FindByIdAsync(dto.UniversityId, cancellationToken)
                ?? throw new DomainValidationException($"University with ID {dto.UniversityId} does not exist.");

            var major = _mapper.Map<UniversityMajor>(dto);
            major.WorldId = university.WorldId; // svijet smjera uvijek = svijet sveučilišta

            var created = await _repository.CreateAsync(major, cancellationToken);
            return _mapper.Map<UniversityMajorDto>(created);
        }
    }

    public class UpdateUniversityMajorCommandHandler : IRequestHandler<UpdateUniversityMajorCommand, UniversityMajorDto>
    {
        private readonly IUniversityMajorRepository _repository;
        private readonly IMapper _mapper;

        public UpdateUniversityMajorCommandHandler(IUniversityMajorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UniversityMajorDto> Handle(UpdateUniversityMajorCommand request, CancellationToken cancellationToken)
        {
            var major = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("UniversityMajor", request.Id);

            _mapper.Map(request.UniversityMajorUpdateDto, major);
            var updated = await _repository.UpdateAsync(major, cancellationToken);
            return _mapper.Map<UniversityMajorDto>(updated);
        }
    }

    public class DeleteUniversityMajorCommandHandler : IRequestHandler<DeleteUniversityMajorCommand, bool>
    {
        private readonly IUniversityMajorRepository _repository;
        private readonly ILogger<DeleteUniversityMajorCommandHandler> _logger;

        public DeleteUniversityMajorCommandHandler(IUniversityMajorRepository repository, ILogger<DeleteUniversityMajorCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteUniversityMajorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting university major with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
