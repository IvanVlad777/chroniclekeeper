using AutoMapper;
using ChronicleKeeper.Core.CQRS.Corporations.Commands;
using ChronicleKeeper.Core.CQRS.Corporations.Queries;
using ChronicleKeeper.Core.DTOs.Corporation;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Corporations.Handlers
{
    public class GetAllCorporationsQueryHandler : IRequestHandler<GetAllCorporationsQuery, List<CorporationDto>>
    {
        private readonly ICorporationRepository _repository;
        private readonly IMapper _mapper;

        public GetAllCorporationsQueryHandler(ICorporationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<CorporationDto>> Handle(GetAllCorporationsQuery request, CancellationToken cancellationToken)
        {
            var corporations = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<CorporationDto>>(corporations);
        }
    }

    public class GetCorporationByIdQueryHandler : IRequestHandler<GetCorporationByIdQuery, CorporationDetailsDto?>
    {
        private readonly ICorporationRepository _repository;
        private readonly IMapper _mapper;

        public GetCorporationByIdQueryHandler(ICorporationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CorporationDetailsDto?> Handle(GetCorporationByIdQuery request, CancellationToken cancellationToken)
        {
            var corporation = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return corporation == null ? null : _mapper.Map<CorporationDetailsDto>(corporation);
        }
    }

    public class CreateCorporationCommandHandler : IRequestHandler<CreateCorporationCommand, CorporationDto>
    {
        private readonly ICorporationRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IIndustryRepository _industryRepository;
        private readonly ITaxationSystemRepository _taxationSystemRepository;
        private readonly IBankingSystemRepository _bankingSystemRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCorporationCommandHandler> _logger;

        public CreateCorporationCommandHandler(
            ICorporationRepository repository,
            IWorldRepository worldRepository,
            IIndustryRepository industryRepository,
            ITaxationSystemRepository taxationSystemRepository,
            IBankingSystemRepository bankingSystemRepository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<CreateCorporationCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _industryRepository = industryRepository;
            _taxationSystemRepository = taxationSystemRepository;
            _bankingSystemRepository = bankingSystemRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CorporationDto> Handle(CreateCorporationCommand request, CancellationToken cancellationToken)
        {
            var dto = request.CorporationCreateDto;
            _logger.LogInformation("Creating new corporation: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            await CorporationValidation.ValidateReferencesAsync(
                dto.IndustryId, dto.TaxationSystemId, dto.BankingSystemId, dto.HistoryId, dto.WorldId,
                _industryRepository, _taxationSystemRepository, _bankingSystemRepository, _historyRepository, cancellationToken);

            if (dto.ParentCorporationId is int parentId)
            {
                var parent = await _repository.FindByIdAsync(parentId, cancellationToken)
                    ?? throw new DomainValidationException($"Corporation with ID {parentId} does not exist.");
                if (parent.WorldId != dto.WorldId)
                {
                    throw new DomainValidationException($"Corporation with ID {parentId} does not belong to this world.");
                }
            }

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Social.Economy.Corporation>(dto), cancellationToken);
            return _mapper.Map<CorporationDto>(created);
        }
    }

    public class UpdateCorporationCommandHandler : IRequestHandler<UpdateCorporationCommand, CorporationDto>
    {
        private readonly ICorporationRepository _repository;
        private readonly IIndustryRepository _industryRepository;
        private readonly ITaxationSystemRepository _taxationSystemRepository;
        private readonly IBankingSystemRepository _bankingSystemRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public UpdateCorporationCommandHandler(
            ICorporationRepository repository,
            IIndustryRepository industryRepository,
            ITaxationSystemRepository taxationSystemRepository,
            IBankingSystemRepository bankingSystemRepository,
            IHistoryRepository historyRepository,
            IMapper mapper)
        {
            _repository = repository;
            _industryRepository = industryRepository;
            _taxationSystemRepository = taxationSystemRepository;
            _bankingSystemRepository = bankingSystemRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public async Task<CorporationDto> Handle(UpdateCorporationCommand request, CancellationToken cancellationToken)
        {
            var corporation = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Corporation", request.Id);

            var dto = request.CorporationUpdateDto;

            await CorporationValidation.ValidateReferencesAsync(
                dto.IndustryId, dto.TaxationSystemId, dto.BankingSystemId, dto.HistoryId, corporation.WorldId,
                _industryRepository, _taxationSystemRepository, _bankingSystemRepository, _historyRepository, cancellationToken);

            if (dto.ParentCorporationId is int parentId)
            {
                if (parentId == corporation.Id)
                {
                    throw new DomainValidationException("A corporation cannot be its own parent.");
                }

                var parent = await _repository.FindByIdAsync(parentId, cancellationToken)
                    ?? throw new DomainValidationException($"Corporation with ID {parentId} does not exist.");
                if (parent.WorldId != corporation.WorldId)
                {
                    throw new DomainValidationException($"Corporation with ID {parentId} does not belong to this world.");
                }

                if (await _repository.WouldCreateCycleAsync(corporation.Id, parentId, cancellationToken))
                {
                    throw new DomainValidationException("This parent assignment would create a cycle in the subsidiary hierarchy.");
                }
            }

            _mapper.Map(dto, corporation);
            var updated = await _repository.UpdateAsync(corporation, cancellationToken);
            return _mapper.Map<CorporationDto>(updated);
        }
    }

    public class DeleteCorporationCommandHandler : IRequestHandler<DeleteCorporationCommand, bool>
    {
        private readonly ICorporationRepository _repository;
        private readonly ILogger<DeleteCorporationCommandHandler> _logger;

        public DeleteCorporationCommandHandler(ICorporationRepository repository, ILogger<DeleteCorporationCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteCorporationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting corporation with ID {Id}", request.Id);

            if (await _repository.HasSubsidiariesAsync(request.Id, cancellationToken))
            {
                throw new DomainValidationException(
                    "This corporation has subsidiaries. Reassign or delete them first.");
            }

            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    public class AddCorporationFactionCommandHandler : IRequestHandler<AddCorporationFactionCommand, bool>
    {
        private readonly ICorporationRepository _repository;
        private readonly IFactionRepository _factionRepository;

        public AddCorporationFactionCommandHandler(ICorporationRepository repository, IFactionRepository factionRepository)
        {
            _repository = repository;
            _factionRepository = factionRepository;
        }

        public async Task<bool> Handle(AddCorporationFactionCommand request, CancellationToken cancellationToken)
        {
            var corporation = await _repository.FindByIdAsync(request.CorporationId, cancellationToken)
                ?? throw new EntityNotFoundException("Corporation", request.CorporationId);

            var faction = await _factionRepository.FindByIdAsync(request.FactionId, cancellationToken)
                ?? throw new DomainValidationException($"Faction with ID {request.FactionId} does not exist.");
            if (faction.WorldId != corporation.WorldId)
            {
                throw new DomainValidationException($"Faction with ID {request.FactionId} does not belong to this world.");
            }

            if (await _repository.IsFactionLinkedAsync(request.CorporationId, request.FactionId, cancellationToken))
            {
                throw new DomainValidationException("This faction is already linked to the corporation.");
            }

            await _repository.AddFactionAsync(request.CorporationId, request.FactionId, cancellationToken);
            return true;
        }
    }

    public class RemoveCorporationFactionCommandHandler : IRequestHandler<RemoveCorporationFactionCommand, bool>
    {
        private readonly ICorporationRepository _repository;

        public RemoveCorporationFactionCommandHandler(ICorporationRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(RemoveCorporationFactionCommand request, CancellationToken cancellationToken)
        {
            return _repository.RemoveFactionAsync(request.CorporationId, request.FactionId, cancellationToken);
        }
    }

    public class AddCorporationProfessionCommandHandler : IRequestHandler<AddCorporationProfessionCommand, bool>
    {
        private readonly ICorporationRepository _repository;
        private readonly IProfessionRepository _professionRepository;

        public AddCorporationProfessionCommandHandler(ICorporationRepository repository, IProfessionRepository professionRepository)
        {
            _repository = repository;
            _professionRepository = professionRepository;
        }

        public async Task<bool> Handle(AddCorporationProfessionCommand request, CancellationToken cancellationToken)
        {
            var corporation = await _repository.FindByIdAsync(request.CorporationId, cancellationToken)
                ?? throw new EntityNotFoundException("Corporation", request.CorporationId);

            var profession = await _professionRepository.FindByIdAsync(request.ProfessionId, cancellationToken)
                ?? throw new DomainValidationException($"Profession with ID {request.ProfessionId} does not exist.");
            if (profession.WorldId != corporation.WorldId)
            {
                throw new DomainValidationException($"Profession with ID {request.ProfessionId} does not belong to this world.");
            }

            if (await _repository.IsProfessionLinkedAsync(request.CorporationId, request.ProfessionId, cancellationToken))
            {
                throw new DomainValidationException("This profession is already linked to the corporation.");
            }

            await _repository.AddProfessionAsync(request.CorporationId, request.ProfessionId, cancellationToken);
            return true;
        }
    }

    public class RemoveCorporationProfessionCommandHandler : IRequestHandler<RemoveCorporationProfessionCommand, bool>
    {
        private readonly ICorporationRepository _repository;

        public RemoveCorporationProfessionCommandHandler(ICorporationRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(RemoveCorporationProfessionCommand request, CancellationToken cancellationToken)
        {
            return _repository.RemoveProfessionAsync(request.CorporationId, request.ProfessionId, cancellationToken);
        }
    }

    internal static class CorporationValidation
    {
        internal static async Task ValidateReferencesAsync(
            int? industryId,
            int? taxationSystemId,
            int? bankingSystemId,
            int? historyId,
            int worldId,
            IIndustryRepository industryRepository,
            ITaxationSystemRepository taxationSystemRepository,
            IBankingSystemRepository bankingSystemRepository,
            IHistoryRepository historyRepository,
            CancellationToken cancellationToken)
        {
            if (industryId is int iid)
            {
                var industry = await industryRepository.FindByIdAsync(iid, cancellationToken)
                    ?? throw new DomainValidationException($"Industry with ID {iid} does not exist.");
                if (industry.WorldId != worldId)
                {
                    throw new DomainValidationException($"Industry with ID {iid} does not belong to this world.");
                }
            }

            if (taxationSystemId is int tid)
            {
                var taxationSystem = await taxationSystemRepository.FindByIdAsync(tid, cancellationToken)
                    ?? throw new DomainValidationException($"Taxation system with ID {tid} does not exist.");
                if (taxationSystem.WorldId != worldId)
                {
                    throw new DomainValidationException($"Taxation system with ID {tid} does not belong to this world.");
                }
            }

            if (bankingSystemId is int bid)
            {
                var bankingSystem = await bankingSystemRepository.FindByIdAsync(bid, cancellationToken)
                    ?? throw new DomainValidationException($"Banking system with ID {bid} does not exist.");
                if (bankingSystem.WorldId != worldId)
                {
                    throw new DomainValidationException($"Banking system with ID {bid} does not belong to this world.");
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
