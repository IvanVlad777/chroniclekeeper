using AutoMapper;
using ChronicleKeeper.Core.CQRS.EducationRecords.Commands;
using ChronicleKeeper.Core.CQRS.EducationRecords.Queries;
using ChronicleKeeper.Core.DTOs.EducationRecord;
using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.EducationRecords.Handlers
{
    public class GetAllEducationRecordsQueryHandler : IRequestHandler<GetAllEducationRecordsQuery, List<EducationRecordDto>>
    {
        private readonly IEducationRecordRepository _repository;
        private readonly IMapper _mapper;

        public GetAllEducationRecordsQueryHandler(IEducationRecordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<EducationRecordDto>> Handle(GetAllEducationRecordsQuery request, CancellationToken cancellationToken)
        {
            var records = await _repository.GetAllAsync(request.WorldId, request.CharacterId, request.SchoolId, request.UniversityId, cancellationToken);
            return _mapper.Map<List<EducationRecordDto>>(records);
        }
    }

    public class GetEducationRecordByIdQueryHandler : IRequestHandler<GetEducationRecordByIdQuery, EducationRecordDto?>
    {
        private readonly IEducationRecordRepository _repository;
        private readonly IMapper _mapper;

        public GetEducationRecordByIdQueryHandler(IEducationRecordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EducationRecordDto?> Handle(GetEducationRecordByIdQuery request, CancellationToken cancellationToken)
        {
            var record = await _repository.FindByIdAsync(request.Id, cancellationToken);
            return record == null ? null : _mapper.Map<EducationRecordDto>(record);
        }
    }

    public class CreateEducationRecordCommandHandler : IRequestHandler<CreateEducationRecordCommand, EducationRecordDto>
    {
        private readonly IEducationRecordRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly ICharacterRepository _characterRepository;
        private readonly ISchoolRepository _schoolRepository;
        private readonly IUniversityRepository _universityRepository;
        private readonly IGuildRepository _guildRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateEducationRecordCommandHandler> _logger;

        public CreateEducationRecordCommandHandler(
            IEducationRecordRepository repository,
            IWorldRepository worldRepository,
            ICharacterRepository characterRepository,
            ISchoolRepository schoolRepository,
            IUniversityRepository universityRepository,
            IGuildRepository guildRepository,
            IMapper mapper,
            ILogger<CreateEducationRecordCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _characterRepository = characterRepository;
            _schoolRepository = schoolRepository;
            _universityRepository = universityRepository;
            _guildRepository = guildRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<EducationRecordDto> Handle(CreateEducationRecordCommand request, CancellationToken cancellationToken)
        {
            var dto = request.EducationRecordCreateDto;
            _logger.LogInformation("Creating new education record: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            if (dto.CharacterId is int characterId
                && !await _characterRepository.ExistsInWorldAsync(characterId, dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"Character with ID {characterId} does not exist in this world.");
            }

            if (dto.SchoolId is int schoolId)
            {
                var school = await _schoolRepository.FindByIdAsync(schoolId, cancellationToken)
                    ?? throw new DomainValidationException($"School with ID {schoolId} does not exist.");
                if (school.WorldId != dto.WorldId)
                {
                    throw new DomainValidationException($"School with ID {schoolId} does not exist in this world.");
                }
            }

            if (dto.UniversityId is int universityId
                && !await _universityRepository.ExistsInWorldAsync(universityId, dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"University with ID {universityId} does not exist in this world.");
            }

            if (dto.GuildId is int guildId)
            {
                var guild = await _guildRepository.FindByIdAsync(guildId, cancellationToken)
                    ?? throw new DomainValidationException($"Guild with ID {guildId} does not exist.");
                if (guild.WorldId != dto.WorldId)
                {
                    throw new DomainValidationException($"Guild with ID {guildId} does not exist in this world.");
                }
            }

            var record = _mapper.Map<EducationRecord>(dto);
            var created = await _repository.CreateAsync(record, cancellationToken);
            return _mapper.Map<EducationRecordDto>(created);
        }
    }

    public class UpdateEducationRecordCommandHandler : IRequestHandler<UpdateEducationRecordCommand, EducationRecordDto>
    {
        private readonly IEducationRecordRepository _repository;
        private readonly ICharacterRepository _characterRepository;
        private readonly ISchoolRepository _schoolRepository;
        private readonly IUniversityRepository _universityRepository;
        private readonly IGuildRepository _guildRepository;
        private readonly IMapper _mapper;

        public UpdateEducationRecordCommandHandler(
            IEducationRecordRepository repository,
            ICharacterRepository characterRepository,
            ISchoolRepository schoolRepository,
            IUniversityRepository universityRepository,
            IGuildRepository guildRepository,
            IMapper mapper)
        {
            _repository = repository;
            _characterRepository = characterRepository;
            _schoolRepository = schoolRepository;
            _universityRepository = universityRepository;
            _guildRepository = guildRepository;
            _mapper = mapper;
        }

        public async Task<EducationRecordDto> Handle(UpdateEducationRecordCommand request, CancellationToken cancellationToken)
        {
            var record = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("EducationRecord", request.Id);

            var dto = request.EducationRecordUpdateDto;

            if (dto.CharacterId is int characterId
                && !await _characterRepository.ExistsInWorldAsync(characterId, record.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"Character with ID {characterId} does not exist in this world.");
            }

            if (dto.SchoolId is int schoolId)
            {
                var school = await _schoolRepository.FindByIdAsync(schoolId, cancellationToken)
                    ?? throw new DomainValidationException($"School with ID {schoolId} does not exist.");
                if (school.WorldId != record.WorldId)
                {
                    throw new DomainValidationException($"School with ID {schoolId} does not exist in this world.");
                }
            }

            if (dto.UniversityId is int universityId
                && !await _universityRepository.ExistsInWorldAsync(universityId, record.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"University with ID {universityId} does not exist in this world.");
            }

            if (dto.GuildId is int guildId)
            {
                var guild = await _guildRepository.FindByIdAsync(guildId, cancellationToken)
                    ?? throw new DomainValidationException($"Guild with ID {guildId} does not exist.");
                if (guild.WorldId != record.WorldId)
                {
                    throw new DomainValidationException($"Guild with ID {guildId} does not exist in this world.");
                }
            }

            _mapper.Map(dto, record);
            var updated = await _repository.UpdateAsync(record, cancellationToken);
            return _mapper.Map<EducationRecordDto>(updated);
        }
    }

    public class DeleteEducationRecordCommandHandler : IRequestHandler<DeleteEducationRecordCommand, bool>
    {
        private readonly IEducationRecordRepository _repository;
        private readonly ILogger<DeleteEducationRecordCommandHandler> _logger;

        public DeleteEducationRecordCommandHandler(IEducationRecordRepository repository, ILogger<DeleteEducationRecordCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteEducationRecordCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting education record with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
