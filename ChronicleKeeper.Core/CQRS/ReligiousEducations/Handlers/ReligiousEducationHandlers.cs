using AutoMapper;
using ChronicleKeeper.Core.CQRS.ReligiousEducations.Commands;
using ChronicleKeeper.Core.CQRS.ReligiousEducations.Queries;
using ChronicleKeeper.Core.DTOs.ReligiousEducation;
using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.ReligiousEducations.Handlers
{
    public class GetAllReligiousEducationsQueryHandler : IRequestHandler<GetAllReligiousEducationsQuery, List<ReligiousEducationDto>>
    {
        private readonly IReligiousEducationRepository _repository;
        private readonly IMapper _mapper;

        public GetAllReligiousEducationsQueryHandler(IReligiousEducationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ReligiousEducationDto>> Handle(GetAllReligiousEducationsQuery request, CancellationToken cancellationToken)
        {
            var records = await _repository.GetAllAsync(request.WorldId, request.CharacterId, request.ReligionId, cancellationToken);
            return _mapper.Map<List<ReligiousEducationDto>>(records);
        }
    }

    public class GetReligiousEducationByIdQueryHandler : IRequestHandler<GetReligiousEducationByIdQuery, ReligiousEducationDto?>
    {
        private readonly IReligiousEducationRepository _repository;
        private readonly IMapper _mapper;

        public GetReligiousEducationByIdQueryHandler(IReligiousEducationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ReligiousEducationDto?> Handle(GetReligiousEducationByIdQuery request, CancellationToken cancellationToken)
        {
            var record = await _repository.FindByIdAsync(request.Id, cancellationToken);
            return record == null ? null : _mapper.Map<ReligiousEducationDto>(record);
        }
    }

    public class CreateReligiousEducationCommandHandler : IRequestHandler<CreateReligiousEducationCommand, ReligiousEducationDto>
    {
        private readonly IReligiousEducationRepository _repository;
        private readonly IReligionRepository _religionRepository;
        private readonly ICharacterRepository _characterRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateReligiousEducationCommandHandler> _logger;

        public CreateReligiousEducationCommandHandler(
            IReligiousEducationRepository repository,
            IReligionRepository religionRepository,
            ICharacterRepository characterRepository,
            IMapper mapper,
            ILogger<CreateReligiousEducationCommandHandler> logger)
        {
            _repository = repository;
            _religionRepository = religionRepository;
            _characterRepository = characterRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReligiousEducationDto> Handle(CreateReligiousEducationCommand request, CancellationToken cancellationToken)
        {
            var dto = request.ReligiousEducationCreateDto;
            _logger.LogInformation("Creating new religious education: {Name}", dto.Name);

            var religion = await _religionRepository.FindByIdAsync(dto.ReligionId, cancellationToken)
                ?? throw new DomainValidationException($"Religion with ID {dto.ReligionId} does not exist.");

            if (dto.CharacterId is int characterId
                && !await _characterRepository.ExistsInWorldAsync(characterId, religion.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"Character with ID {characterId} does not exist in this world.");
            }

            var religiousEducation = _mapper.Map<ReligiousEducation>(dto);
            religiousEducation.WorldId = religion.WorldId; // svijet vjerskog obrazovanja uvijek = svijet religije

            var created = await _repository.CreateAsync(religiousEducation, cancellationToken);
            return _mapper.Map<ReligiousEducationDto>(created);
        }
    }

    public class UpdateReligiousEducationCommandHandler : IRequestHandler<UpdateReligiousEducationCommand, ReligiousEducationDto>
    {
        private readonly IReligiousEducationRepository _repository;
        private readonly ICharacterRepository _characterRepository;
        private readonly IMapper _mapper;

        public UpdateReligiousEducationCommandHandler(
            IReligiousEducationRepository repository,
            ICharacterRepository characterRepository,
            IMapper mapper)
        {
            _repository = repository;
            _characterRepository = characterRepository;
            _mapper = mapper;
        }

        public async Task<ReligiousEducationDto> Handle(UpdateReligiousEducationCommand request, CancellationToken cancellationToken)
        {
            var religiousEducation = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("ReligiousEducation", request.Id);

            var dto = request.ReligiousEducationUpdateDto;

            if (dto.CharacterId is int characterId
                && !await _characterRepository.ExistsInWorldAsync(characterId, religiousEducation.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"Character with ID {characterId} does not exist in this world.");
            }

            _mapper.Map(dto, religiousEducation);
            var updated = await _repository.UpdateAsync(religiousEducation, cancellationToken);
            return _mapper.Map<ReligiousEducationDto>(updated);
        }
    }

    public class DeleteReligiousEducationCommandHandler : IRequestHandler<DeleteReligiousEducationCommand, bool>
    {
        private readonly IReligiousEducationRepository _repository;
        private readonly ILogger<DeleteReligiousEducationCommandHandler> _logger;

        public DeleteReligiousEducationCommandHandler(IReligiousEducationRepository repository, ILogger<DeleteReligiousEducationCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteReligiousEducationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting religious education with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
