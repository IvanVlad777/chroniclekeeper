using AutoMapper;
using ChronicleKeeper.Core.CQRS.Schools.Commands;
using ChronicleKeeper.Core.CQRS.Schools.Queries;
using ChronicleKeeper.Core.DTOs.School;
using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Schools.Handlers
{
    public class GetAllSchoolsQueryHandler : IRequestHandler<GetAllSchoolsQuery, List<SchoolDto>>
    {
        private readonly ISchoolRepository _repository;
        private readonly IMapper _mapper;

        public GetAllSchoolsQueryHandler(ISchoolRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<SchoolDto>> Handle(GetAllSchoolsQuery request, CancellationToken cancellationToken)
        {
            var schools = await _repository.GetAllAsync(request.WorldId, request.EducationSystemId, cancellationToken);
            return _mapper.Map<List<SchoolDto>>(schools);
        }
    }

    public class GetSchoolByIdQueryHandler : IRequestHandler<GetSchoolByIdQuery, SchoolDetailsDto?>
    {
        private readonly ISchoolRepository _repository;
        private readonly IMapper _mapper;

        public GetSchoolByIdQueryHandler(ISchoolRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SchoolDetailsDto?> Handle(GetSchoolByIdQuery request, CancellationToken cancellationToken)
        {
            var school = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return school == null ? null : _mapper.Map<SchoolDetailsDto>(school);
        }
    }

    public class CreateSchoolCommandHandler : IRequestHandler<CreateSchoolCommand, SchoolDto>
    {
        private readonly ISchoolRepository _repository;
        private readonly IEducationSystemRepository _educationSystemRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSchoolCommandHandler> _logger;

        public CreateSchoolCommandHandler(
            ISchoolRepository repository,
            IEducationSystemRepository educationSystemRepository,
            ILocationRepository locationRepository,
            IMapper mapper,
            ILogger<CreateSchoolCommandHandler> logger)
        {
            _repository = repository;
            _educationSystemRepository = educationSystemRepository;
            _locationRepository = locationRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SchoolDto> Handle(CreateSchoolCommand request, CancellationToken cancellationToken)
        {
            var dto = request.SchoolCreateDto;
            _logger.LogInformation("Creating new school: {Name}", dto.Name);

            var educationSystem = await _educationSystemRepository.FindByIdAsync(dto.EducationSystemId, cancellationToken)
                ?? throw new DomainValidationException($"Education system with ID {dto.EducationSystemId} does not exist.");

            var school = _mapper.Map<School>(dto);
            school.WorldId = educationSystem.WorldId; // svijet škole uvijek = svijet sustava obrazovanja

            await SchoolValidation.ValidateLocationAsync(_locationRepository, dto.LocationId, school.WorldId, cancellationToken);

            var created = await _repository.CreateAsync(school, cancellationToken);
            return _mapper.Map<SchoolDto>(created);
        }
    }

    public class UpdateSchoolCommandHandler : IRequestHandler<UpdateSchoolCommand, SchoolDto>
    {
        private readonly ISchoolRepository _repository;
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public UpdateSchoolCommandHandler(ISchoolRepository repository, ILocationRepository locationRepository, IMapper mapper)
        {
            _repository = repository;
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public async Task<SchoolDto> Handle(UpdateSchoolCommand request, CancellationToken cancellationToken)
        {
            var school = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("School", request.Id);

            await SchoolValidation.ValidateLocationAsync(_locationRepository, request.SchoolUpdateDto.LocationId, school.WorldId, cancellationToken);

            _mapper.Map(request.SchoolUpdateDto, school);
            var updated = await _repository.UpdateAsync(school, cancellationToken);
            return _mapper.Map<SchoolDto>(updated);
        }
    }

    public class DeleteSchoolCommandHandler : IRequestHandler<DeleteSchoolCommand, bool>
    {
        private readonly ISchoolRepository _repository;
        private readonly ILogger<DeleteSchoolCommandHandler> _logger;

        public DeleteSchoolCommandHandler(ISchoolRepository repository, ILogger<DeleteSchoolCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteSchoolCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting school with ID {Id}", request.Id);

            var inUse = await _repository.CountEducationRecordsUsingSchoolAsync(request.Id, cancellationToken);
            if (inUse > 0)
            {
                throw new DomainValidationException(
                    $"This school is used by {inUse} education record(s). Remove them first.");
            }

            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    internal static class SchoolValidation
    {
        public static async Task ValidateLocationAsync(
            ILocationRepository locationRepository, int? locationId, int worldId, CancellationToken cancellationToken)
        {
            if (locationId is not int id) return;

            if (!await locationRepository.ExistsInWorldAsync(id, worldId, cancellationToken))
            {
                throw new DomainValidationException($"Location with ID {id} does not exist in this world.");
            }
        }
    }
}
