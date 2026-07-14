using AutoMapper;
using ChronicleKeeper.Core.CQRS.CorporateLeaderships.Commands;
using ChronicleKeeper.Core.CQRS.CorporateLeaderships.Queries;
using ChronicleKeeper.Core.DTOs.CorporateLeadership;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.CorporateLeaderships.Handlers
{
    public class GetAllCorporateLeadershipsQueryHandler : IRequestHandler<GetAllCorporateLeadershipsQuery, List<CorporateLeadershipDto>>
    {
        private readonly ICorporateLeadershipRepository _repository;
        private readonly IMapper _mapper;

        public GetAllCorporateLeadershipsQueryHandler(ICorporateLeadershipRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<CorporateLeadershipDto>> Handle(GetAllCorporateLeadershipsQuery request, CancellationToken cancellationToken)
        {
            var leaderships = await _repository.GetAllAsync(request.WorldId, request.CorporationId, cancellationToken);
            return _mapper.Map<List<CorporateLeadershipDto>>(leaderships);
        }
    }

    public class GetCorporateLeadershipByIdQueryHandler : IRequestHandler<GetCorporateLeadershipByIdQuery, CorporateLeadershipDto?>
    {
        private readonly ICorporateLeadershipRepository _repository;
        private readonly IMapper _mapper;

        public GetCorporateLeadershipByIdQueryHandler(ICorporateLeadershipRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CorporateLeadershipDto?> Handle(GetCorporateLeadershipByIdQuery request, CancellationToken cancellationToken)
        {
            var leadership = await _repository.FindByIdAsync(request.Id, cancellationToken);
            return leadership == null ? null : _mapper.Map<CorporateLeadershipDto>(leadership);
        }
    }

    public class CreateCorporateLeadershipCommandHandler : IRequestHandler<CreateCorporateLeadershipCommand, CorporateLeadershipDto>
    {
        private readonly ICorporateLeadershipRepository _repository;
        private readonly ICorporationRepository _corporationRepository;
        private readonly IProfessionRepository _professionRepository;
        private readonly ICharacterRepository _characterRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCorporateLeadershipCommandHandler> _logger;

        public CreateCorporateLeadershipCommandHandler(
            ICorporateLeadershipRepository repository,
            ICorporationRepository corporationRepository,
            IProfessionRepository professionRepository,
            ICharacterRepository characterRepository,
            IMapper mapper,
            ILogger<CreateCorporateLeadershipCommandHandler> logger)
        {
            _repository = repository;
            _corporationRepository = corporationRepository;
            _professionRepository = professionRepository;
            _characterRepository = characterRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CorporateLeadershipDto> Handle(CreateCorporateLeadershipCommand request, CancellationToken cancellationToken)
        {
            var dto = request.CorporateLeadershipCreateDto;
            _logger.LogInformation("Creating new corporate leadership entry: {Name}", dto.Name);

            var corporation = await _corporationRepository.FindByIdAsync(dto.CorporationId, cancellationToken)
                ?? throw new DomainValidationException($"Corporation with ID {dto.CorporationId} does not exist.");

            await CorporateLeadershipValidation.ValidateReferencesAsync(
                dto.ProfessionId, dto.CharacterId, corporation.WorldId, _professionRepository, _characterRepository, cancellationToken);

            var leadership = _mapper.Map<Entities.Social.Economy.CorporateLeadership>(dto);
            leadership.WorldId = corporation.WorldId; // svijet pozicije uvijek = svijet korporacije

            var created = await _repository.CreateAsync(leadership, cancellationToken);
            return _mapper.Map<CorporateLeadershipDto>(created);
        }
    }

    public class UpdateCorporateLeadershipCommandHandler : IRequestHandler<UpdateCorporateLeadershipCommand, CorporateLeadershipDto>
    {
        private readonly ICorporateLeadershipRepository _repository;
        private readonly IProfessionRepository _professionRepository;
        private readonly ICharacterRepository _characterRepository;
        private readonly IMapper _mapper;

        public UpdateCorporateLeadershipCommandHandler(
            ICorporateLeadershipRepository repository,
            IProfessionRepository professionRepository,
            ICharacterRepository characterRepository,
            IMapper mapper)
        {
            _repository = repository;
            _professionRepository = professionRepository;
            _characterRepository = characterRepository;
            _mapper = mapper;
        }

        public async Task<CorporateLeadershipDto> Handle(UpdateCorporateLeadershipCommand request, CancellationToken cancellationToken)
        {
            var leadership = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("CorporateLeadership", request.Id);

            var dto = request.CorporateLeadershipUpdateDto;

            await CorporateLeadershipValidation.ValidateReferencesAsync(
                dto.ProfessionId, dto.CharacterId, leadership.WorldId, _professionRepository, _characterRepository, cancellationToken);

            _mapper.Map(dto, leadership);
            var updated = await _repository.UpdateAsync(leadership, cancellationToken);
            return _mapper.Map<CorporateLeadershipDto>(updated);
        }
    }

    public class DeleteCorporateLeadershipCommandHandler : IRequestHandler<DeleteCorporateLeadershipCommand, bool>
    {
        private readonly ICorporateLeadershipRepository _repository;
        private readonly ILogger<DeleteCorporateLeadershipCommandHandler> _logger;

        public DeleteCorporateLeadershipCommandHandler(ICorporateLeadershipRepository repository, ILogger<DeleteCorporateLeadershipCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteCorporateLeadershipCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting corporate leadership entry with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    internal static class CorporateLeadershipValidation
    {
        internal static async Task ValidateReferencesAsync(
            int? professionId,
            int? characterId,
            int worldId,
            IProfessionRepository professionRepository,
            ICharacterRepository characterRepository,
            CancellationToken cancellationToken)
        {
            if (professionId is int pid)
            {
                var profession = await professionRepository.FindByIdAsync(pid, cancellationToken)
                    ?? throw new DomainValidationException($"Profession with ID {pid} does not exist.");
                if (profession.WorldId != worldId)
                {
                    throw new DomainValidationException($"Profession with ID {pid} does not belong to this world.");
                }
            }

            if (characterId is int cid)
            {
                var character = await characterRepository.FindByIdAsync(cid, cancellationToken)
                    ?? throw new DomainValidationException($"Character with ID {cid} does not exist.");
                if (character.WorldId != worldId)
                {
                    throw new DomainValidationException($"Character with ID {cid} does not belong to this world.");
                }
            }
        }
    }
}
