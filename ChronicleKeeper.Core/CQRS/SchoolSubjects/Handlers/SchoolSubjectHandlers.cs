using AutoMapper;
using ChronicleKeeper.Core.CQRS.SchoolSubjects.Commands;
using ChronicleKeeper.Core.CQRS.SchoolSubjects.Queries;
using ChronicleKeeper.Core.DTOs.SchoolSubject;
using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.SchoolSubjects.Handlers
{
    public class GetAllSchoolSubjectsQueryHandler : IRequestHandler<GetAllSchoolSubjectsQuery, List<SchoolSubjectDto>>
    {
        private readonly ISchoolSubjectRepository _repository;
        private readonly IMapper _mapper;

        public GetAllSchoolSubjectsQueryHandler(ISchoolSubjectRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<SchoolSubjectDto>> Handle(GetAllSchoolSubjectsQuery request, CancellationToken cancellationToken)
        {
            var subjects = await _repository.GetAllAsync(request.WorldId, request.SchoolId, cancellationToken);
            return _mapper.Map<List<SchoolSubjectDto>>(subjects);
        }
    }

    public class GetSchoolSubjectByIdQueryHandler : IRequestHandler<GetSchoolSubjectByIdQuery, SchoolSubjectDetailsDto?>
    {
        private readonly ISchoolSubjectRepository _repository;
        private readonly IMapper _mapper;

        public GetSchoolSubjectByIdQueryHandler(ISchoolSubjectRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SchoolSubjectDetailsDto?> Handle(GetSchoolSubjectByIdQuery request, CancellationToken cancellationToken)
        {
            var subject = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return subject == null ? null : _mapper.Map<SchoolSubjectDetailsDto>(subject);
        }
    }

    public class AddSchoolSubjectTeacherCommandHandler : IRequestHandler<AddSchoolSubjectTeacherCommand, bool>
    {
        private readonly ISchoolSubjectRepository _repository;
        private readonly ICharacterRepository _characterRepository;

        public AddSchoolSubjectTeacherCommandHandler(ISchoolSubjectRepository repository, ICharacterRepository characterRepository)
        {
            _repository = repository;
            _characterRepository = characterRepository;
        }

        public async Task<bool> Handle(AddSchoolSubjectTeacherCommand request, CancellationToken cancellationToken)
        {
            var subject = await _repository.FindByIdAsync(request.SchoolSubjectId, cancellationToken)
                ?? throw new EntityNotFoundException("SchoolSubject", request.SchoolSubjectId);

            if (!await _characterRepository.ExistsInWorldAsync(request.CharacterId, subject.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"Character with ID {request.CharacterId} does not exist in this world.");
            }

            if (await _repository.IsTeacherLinkedAsync(request.SchoolSubjectId, request.CharacterId, cancellationToken))
            {
                throw new DomainValidationException("This character is already a teacher of the subject.");
            }

            await _repository.AddTeacherAsync(request.SchoolSubjectId, request.CharacterId, cancellationToken);
            return true;
        }
    }

    public class RemoveSchoolSubjectTeacherCommandHandler : IRequestHandler<RemoveSchoolSubjectTeacherCommand, bool>
    {
        private readonly ISchoolSubjectRepository _repository;

        public RemoveSchoolSubjectTeacherCommandHandler(ISchoolSubjectRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(RemoveSchoolSubjectTeacherCommand request, CancellationToken cancellationToken)
        {
            return _repository.RemoveTeacherAsync(request.SchoolSubjectId, request.CharacterId, cancellationToken);
        }
    }

    public class CreateSchoolSubjectCommandHandler : IRequestHandler<CreateSchoolSubjectCommand, SchoolSubjectDto>
    {
        private readonly ISchoolSubjectRepository _repository;
        private readonly ISchoolRepository _schoolRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSchoolSubjectCommandHandler> _logger;

        public CreateSchoolSubjectCommandHandler(
            ISchoolSubjectRepository repository,
            ISchoolRepository schoolRepository,
            IMapper mapper,
            ILogger<CreateSchoolSubjectCommandHandler> logger)
        {
            _repository = repository;
            _schoolRepository = schoolRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SchoolSubjectDto> Handle(CreateSchoolSubjectCommand request, CancellationToken cancellationToken)
        {
            var dto = request.SchoolSubjectCreateDto;
            _logger.LogInformation("Creating new school subject: {Name}", dto.Name);

            var school = await _schoolRepository.FindByIdAsync(dto.SchoolId, cancellationToken)
                ?? throw new DomainValidationException($"School with ID {dto.SchoolId} does not exist.");

            var subject = _mapper.Map<SchoolSubject>(dto);
            subject.WorldId = school.WorldId; // svijet predmeta uvijek = svijet škole

            var created = await _repository.CreateAsync(subject, cancellationToken);
            return _mapper.Map<SchoolSubjectDto>(created);
        }
    }

    public class UpdateSchoolSubjectCommandHandler : IRequestHandler<UpdateSchoolSubjectCommand, SchoolSubjectDto>
    {
        private readonly ISchoolSubjectRepository _repository;
        private readonly IMapper _mapper;

        public UpdateSchoolSubjectCommandHandler(ISchoolSubjectRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SchoolSubjectDto> Handle(UpdateSchoolSubjectCommand request, CancellationToken cancellationToken)
        {
            var subject = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("SchoolSubject", request.Id);

            _mapper.Map(request.SchoolSubjectUpdateDto, subject);
            var updated = await _repository.UpdateAsync(subject, cancellationToken);
            return _mapper.Map<SchoolSubjectDto>(updated);
        }
    }

    public class DeleteSchoolSubjectCommandHandler : IRequestHandler<DeleteSchoolSubjectCommand, bool>
    {
        private readonly ISchoolSubjectRepository _repository;
        private readonly ILogger<DeleteSchoolSubjectCommandHandler> _logger;

        public DeleteSchoolSubjectCommandHandler(ISchoolSubjectRepository repository, ILogger<DeleteSchoolSubjectCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteSchoolSubjectCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting school subject with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
