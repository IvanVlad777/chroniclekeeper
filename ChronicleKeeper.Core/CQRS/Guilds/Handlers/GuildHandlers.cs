using AutoMapper;
using ChronicleKeeper.Core.CQRS.Guilds.Commands;
using ChronicleKeeper.Core.CQRS.Guilds.Queries;
using ChronicleKeeper.Core.DTOs.Guild;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Guilds.Handlers
{
    public class GetAllGuildsQueryHandler : IRequestHandler<GetAllGuildsQuery, List<GuildDto>>
    {
        private readonly IGuildRepository _repository;
        private readonly IMapper _mapper;

        public GetAllGuildsQueryHandler(IGuildRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<GuildDto>> Handle(GetAllGuildsQuery request, CancellationToken cancellationToken)
        {
            var guilds = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<GuildDto>>(guilds);
        }
    }

    public class GetGuildByIdQueryHandler : IRequestHandler<GetGuildByIdQuery, GuildDetailsDto?>
    {
        private readonly IGuildRepository _repository;
        private readonly IMapper _mapper;

        public GetGuildByIdQueryHandler(IGuildRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GuildDetailsDto?> Handle(GetGuildByIdQuery request, CancellationToken cancellationToken)
        {
            var guild = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return guild == null ? null : _mapper.Map<GuildDetailsDto>(guild);
        }
    }

    public class CreateGuildCommandHandler : IRequestHandler<CreateGuildCommand, GuildDto>
    {
        private readonly IGuildRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly ITaxationSystemRepository _taxationSystemRepository;
        private readonly IIndustryRepository _industryRepository;
        private readonly ILegalSystemRepository _legalSystemRepository;
        private readonly IEducationSystemRepository _educationSystemRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateGuildCommandHandler> _logger;

        public CreateGuildCommandHandler(
            IGuildRepository repository,
            IWorldRepository worldRepository,
            ITaxationSystemRepository taxationSystemRepository,
            IIndustryRepository industryRepository,
            ILegalSystemRepository legalSystemRepository,
            IEducationSystemRepository educationSystemRepository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<CreateGuildCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _taxationSystemRepository = taxationSystemRepository;
            _industryRepository = industryRepository;
            _legalSystemRepository = legalSystemRepository;
            _educationSystemRepository = educationSystemRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GuildDto> Handle(CreateGuildCommand request, CancellationToken cancellationToken)
        {
            var dto = request.GuildCreateDto;
            _logger.LogInformation("Creating new guild: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            await GuildValidation.ValidateReferencesAsync(
                dto.TaxationSystemId, dto.IndustryId, dto.LegalSystemId, dto.EducationSystemId, dto.HistoryId, dto.WorldId,
                _taxationSystemRepository, _industryRepository, _legalSystemRepository, _educationSystemRepository, _historyRepository,
                cancellationToken);

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Social.Economy.Guild>(dto), cancellationToken);
            return _mapper.Map<GuildDto>(created);
        }
    }

    public class UpdateGuildCommandHandler : IRequestHandler<UpdateGuildCommand, GuildDto>
    {
        private readonly IGuildRepository _repository;
        private readonly ITaxationSystemRepository _taxationSystemRepository;
        private readonly IIndustryRepository _industryRepository;
        private readonly ILegalSystemRepository _legalSystemRepository;
        private readonly IEducationSystemRepository _educationSystemRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public UpdateGuildCommandHandler(
            IGuildRepository repository,
            ITaxationSystemRepository taxationSystemRepository,
            IIndustryRepository industryRepository,
            ILegalSystemRepository legalSystemRepository,
            IEducationSystemRepository educationSystemRepository,
            IHistoryRepository historyRepository,
            IMapper mapper)
        {
            _repository = repository;
            _taxationSystemRepository = taxationSystemRepository;
            _industryRepository = industryRepository;
            _legalSystemRepository = legalSystemRepository;
            _educationSystemRepository = educationSystemRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public async Task<GuildDto> Handle(UpdateGuildCommand request, CancellationToken cancellationToken)
        {
            var guild = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Guild", request.Id);

            var dto = request.GuildUpdateDto;

            await GuildValidation.ValidateReferencesAsync(
                dto.TaxationSystemId, dto.IndustryId, dto.LegalSystemId, dto.EducationSystemId, dto.HistoryId, guild.WorldId,
                _taxationSystemRepository, _industryRepository, _legalSystemRepository, _educationSystemRepository, _historyRepository,
                cancellationToken);

            _mapper.Map(dto, guild);
            var updated = await _repository.UpdateAsync(guild, cancellationToken);
            return _mapper.Map<GuildDto>(updated);
        }
    }

    public class DeleteGuildCommandHandler : IRequestHandler<DeleteGuildCommand, bool>
    {
        private readonly IGuildRepository _repository;
        private readonly ILogger<DeleteGuildCommandHandler> _logger;

        public DeleteGuildCommandHandler(IGuildRepository repository, ILogger<DeleteGuildCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteGuildCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting guild with ID {Id}", request.Id);
            // Nema guardova: GuildRanks + join tablice kaskadiraju, Apprenticeship/EducationRecord su SetNull.
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    public class AddGuildFactionCommandHandler : IRequestHandler<AddGuildFactionCommand, bool>
    {
        private readonly IGuildRepository _repository;
        private readonly IFactionRepository _factionRepository;

        public AddGuildFactionCommandHandler(IGuildRepository repository, IFactionRepository factionRepository)
        {
            _repository = repository;
            _factionRepository = factionRepository;
        }

        public async Task<bool> Handle(AddGuildFactionCommand request, CancellationToken cancellationToken)
        {
            var guild = await _repository.FindByIdAsync(request.GuildId, cancellationToken)
                ?? throw new EntityNotFoundException("Guild", request.GuildId);

            var faction = await _factionRepository.FindByIdAsync(request.FactionId, cancellationToken)
                ?? throw new DomainValidationException($"Faction with ID {request.FactionId} does not exist.");
            if (faction.WorldId != guild.WorldId)
            {
                throw new DomainValidationException($"Faction with ID {request.FactionId} does not belong to this world.");
            }

            if (await _repository.IsFactionLinkedAsync(request.GuildId, request.FactionId, cancellationToken))
            {
                throw new DomainValidationException("This faction is already linked to the guild.");
            }

            await _repository.AddFactionAsync(request.GuildId, request.FactionId, cancellationToken);
            return true;
        }
    }

    public class RemoveGuildFactionCommandHandler : IRequestHandler<RemoveGuildFactionCommand, bool>
    {
        private readonly IGuildRepository _repository;

        public RemoveGuildFactionCommandHandler(IGuildRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(RemoveGuildFactionCommand request, CancellationToken cancellationToken)
        {
            return _repository.RemoveFactionAsync(request.GuildId, request.FactionId, cancellationToken);
        }
    }

    public class AddGuildProfessionCommandHandler : IRequestHandler<AddGuildProfessionCommand, bool>
    {
        private readonly IGuildRepository _repository;
        private readonly IProfessionRepository _professionRepository;

        public AddGuildProfessionCommandHandler(IGuildRepository repository, IProfessionRepository professionRepository)
        {
            _repository = repository;
            _professionRepository = professionRepository;
        }

        public async Task<bool> Handle(AddGuildProfessionCommand request, CancellationToken cancellationToken)
        {
            var guild = await _repository.FindByIdAsync(request.GuildId, cancellationToken)
                ?? throw new EntityNotFoundException("Guild", request.GuildId);

            var profession = await _professionRepository.FindByIdAsync(request.ProfessionId, cancellationToken)
                ?? throw new DomainValidationException($"Profession with ID {request.ProfessionId} does not exist.");
            if (profession.WorldId != guild.WorldId)
            {
                throw new DomainValidationException($"Profession with ID {request.ProfessionId} does not belong to this world.");
            }

            if (await _repository.IsProfessionLinkedAsync(request.GuildId, request.ProfessionId, cancellationToken))
            {
                throw new DomainValidationException("This profession is already linked to the guild.");
            }

            await _repository.AddProfessionAsync(request.GuildId, request.ProfessionId, cancellationToken);
            return true;
        }
    }

    public class RemoveGuildProfessionCommandHandler : IRequestHandler<RemoveGuildProfessionCommand, bool>
    {
        private readonly IGuildRepository _repository;

        public RemoveGuildProfessionCommandHandler(IGuildRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(RemoveGuildProfessionCommand request, CancellationToken cancellationToken)
        {
            return _repository.RemoveProfessionAsync(request.GuildId, request.ProfessionId, cancellationToken);
        }
    }

    public class AddGuildSocialClassCommandHandler : IRequestHandler<AddGuildSocialClassCommand, bool>
    {
        private readonly IGuildRepository _repository;
        private readonly ISocialClassRepository _socialClassRepository;

        public AddGuildSocialClassCommandHandler(IGuildRepository repository, ISocialClassRepository socialClassRepository)
        {
            _repository = repository;
            _socialClassRepository = socialClassRepository;
        }

        public async Task<bool> Handle(AddGuildSocialClassCommand request, CancellationToken cancellationToken)
        {
            var guild = await _repository.FindByIdAsync(request.GuildId, cancellationToken)
                ?? throw new EntityNotFoundException("Guild", request.GuildId);

            var socialClass = await _socialClassRepository.FindByIdAsync(request.SocialClassId, cancellationToken)
                ?? throw new DomainValidationException($"Social class with ID {request.SocialClassId} does not exist.");
            if (socialClass.WorldId != guild.WorldId)
            {
                throw new DomainValidationException($"Social class with ID {request.SocialClassId} does not belong to this world.");
            }

            if (await _repository.IsSocialClassLinkedAsync(request.GuildId, request.SocialClassId, cancellationToken))
            {
                throw new DomainValidationException("This social class is already linked to the guild.");
            }

            await _repository.AddSocialClassAsync(request.GuildId, request.SocialClassId, cancellationToken);
            return true;
        }
    }

    public class RemoveGuildSocialClassCommandHandler : IRequestHandler<RemoveGuildSocialClassCommand, bool>
    {
        private readonly IGuildRepository _repository;

        public RemoveGuildSocialClassCommandHandler(IGuildRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(RemoveGuildSocialClassCommand request, CancellationToken cancellationToken)
        {
            return _repository.RemoveSocialClassAsync(request.GuildId, request.SocialClassId, cancellationToken);
        }
    }

    internal static class GuildValidation
    {
        internal static async Task ValidateReferencesAsync(
            int? taxationSystemId,
            int? industryId,
            int? legalSystemId,
            int? educationSystemId,
            int? historyId,
            int worldId,
            ITaxationSystemRepository taxationSystemRepository,
            IIndustryRepository industryRepository,
            ILegalSystemRepository legalSystemRepository,
            IEducationSystemRepository educationSystemRepository,
            IHistoryRepository historyRepository,
            CancellationToken cancellationToken)
        {
            if (taxationSystemId is int tid)
            {
                var taxationSystem = await taxationSystemRepository.FindByIdAsync(tid, cancellationToken)
                    ?? throw new DomainValidationException($"Taxation system with ID {tid} does not exist.");
                if (taxationSystem.WorldId != worldId)
                {
                    throw new DomainValidationException($"Taxation system with ID {tid} does not belong to this world.");
                }
            }

            if (industryId is int iid)
            {
                var industry = await industryRepository.FindByIdAsync(iid, cancellationToken)
                    ?? throw new DomainValidationException($"Industry with ID {iid} does not exist.");
                if (industry.WorldId != worldId)
                {
                    throw new DomainValidationException($"Industry with ID {iid} does not belong to this world.");
                }
            }

            if (legalSystemId is int lid)
            {
                var legalSystem = await legalSystemRepository.FindByIdAsync(lid, cancellationToken)
                    ?? throw new DomainValidationException($"Legal system with ID {lid} does not exist.");
                if (legalSystem.WorldId != worldId)
                {
                    throw new DomainValidationException($"Legal system with ID {lid} does not belong to this world.");
                }
            }

            if (educationSystemId is int eid)
            {
                var educationSystem = await educationSystemRepository.FindByIdAsync(eid, cancellationToken)
                    ?? throw new DomainValidationException($"Education system with ID {eid} does not exist.");
                if (educationSystem.WorldId != worldId)
                {
                    throw new DomainValidationException($"Education system with ID {eid} does not belong to this world.");
                }
            }

            if (historyId is int hid)
            {
                var history = await historyRepository.FindByIdAsync(hid, cancellationToken)
                    ?? throw new DomainValidationException($"History with ID {hid} does not exist.");
                if (history.WorldId != worldId)
                {
                    throw new DomainValidationException($"History with ID {hid} does not belong to this world.");
                }
            }
        }
    }
}
