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

    public class GetUniversityMajorByIdQueryHandler : IRequestHandler<GetUniversityMajorByIdQuery, UniversityMajorDto?>
    {
        private readonly IUniversityMajorRepository _repository;
        private readonly IMapper _mapper;

        public GetUniversityMajorByIdQueryHandler(IUniversityMajorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UniversityMajorDto?> Handle(GetUniversityMajorByIdQuery request, CancellationToken cancellationToken)
        {
            var major = await _repository.FindByIdAsync(request.Id, cancellationToken);
            return major == null ? null : _mapper.Map<UniversityMajorDto>(major);
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
