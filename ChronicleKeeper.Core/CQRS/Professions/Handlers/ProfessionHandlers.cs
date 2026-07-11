using AutoMapper;
using ChronicleKeeper.Core.CQRS.Professions.Commands;
using ChronicleKeeper.Core.CQRS.Professions.Queries;
using ChronicleKeeper.Core.DTOs.Profession;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Professions.Handlers
{
    public class GetAllProfessionsQueryHandler : IRequestHandler<GetAllProfessionsQuery, List<ProfessionDto>>
    {
        private readonly IProfessionRepository _repository;
        private readonly IMapper _mapper;

        public GetAllProfessionsQueryHandler(IProfessionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ProfessionDto>> Handle(GetAllProfessionsQuery request, CancellationToken cancellationToken)
        {
            var professions = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<ProfessionDto>>(professions);
        }
    }

    public class GetProfessionByIdQueryHandler : IRequestHandler<GetProfessionByIdQuery, ProfessionDetailsDto?>
    {
        private readonly IProfessionRepository _repository;
        private readonly IMapper _mapper;

        public GetProfessionByIdQueryHandler(IProfessionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProfessionDetailsDto?> Handle(GetProfessionByIdQuery request, CancellationToken cancellationToken)
        {
            var profession = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return profession == null ? null : _mapper.Map<ProfessionDetailsDto>(profession);
        }
    }

    public class CreateProfessionCommandHandler : IRequestHandler<CreateProfessionCommand, ProfessionDto>
    {
        private readonly IProfessionRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateProfessionCommandHandler> _logger;

        public CreateProfessionCommandHandler(
            IProfessionRepository repository,
            IWorldRepository worldRepository,
            IMapper mapper,
            ILogger<CreateProfessionCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ProfessionDto> Handle(CreateProfessionCommand request, CancellationToken cancellationToken)
        {
            var dto = request.ProfessionCreateDto;
            _logger.LogInformation("Creating new profession: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Professions.Profession>(dto), cancellationToken);
            return _mapper.Map<ProfessionDto>(created);
        }
    }

    public class UpdateProfessionCommandHandler : IRequestHandler<UpdateProfessionCommand, ProfessionDto>
    {
        private readonly IProfessionRepository _repository;
        private readonly IMapper _mapper;

        public UpdateProfessionCommandHandler(IProfessionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProfessionDto> Handle(UpdateProfessionCommand request, CancellationToken cancellationToken)
        {
            var profession = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Profession", request.Id);

            _mapper.Map(request.ProfessionUpdateDto, profession);
            var updated = await _repository.UpdateAsync(profession, cancellationToken);
            return _mapper.Map<ProfessionDto>(updated);
        }
    }

    public class DeleteProfessionCommandHandler : IRequestHandler<DeleteProfessionCommand, bool>
    {
        private readonly IProfessionRepository _repository;
        private readonly ILogger<DeleteProfessionCommandHandler> _logger;

        public DeleteProfessionCommandHandler(IProfessionRepository repository, ILogger<DeleteProfessionCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteProfessionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting profession with ID {Id}", request.Id);

            var inUse = await _repository.CountCharactersUsingProfessionAsync(request.Id, cancellationToken);
            if (inUse > 0)
            {
                throw new DomainValidationException(
                    $"This profession is used by {inUse} character(s). Reassign them first.");
            }

            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    // ----- Species link -----

    public class AddProfessionSpeciesCommandHandler : IRequestHandler<AddProfessionSpeciesCommand, bool>
    {
        private readonly IProfessionRepository _repository;
        private readonly ISpeciesRepository _speciesRepository;

        public AddProfessionSpeciesCommandHandler(IProfessionRepository repository, ISpeciesRepository speciesRepository)
        {
            _repository = repository;
            _speciesRepository = speciesRepository;
        }

        public async Task<bool> Handle(AddProfessionSpeciesCommand request, CancellationToken cancellationToken)
        {
            var profession = await _repository.FindByIdAsync(request.ProfessionId, cancellationToken)
                ?? throw new EntityNotFoundException("Profession", request.ProfessionId);

            var species = await _speciesRepository.FindByIdAsync(request.SpeciesId, cancellationToken)
                ?? throw new DomainValidationException($"Species with ID {request.SpeciesId} does not exist.");
            if (species.WorldId != profession.WorldId)
            {
                throw new DomainValidationException($"Species with ID {request.SpeciesId} does not belong to this world.");
            }

            if (await _repository.IsSpeciesLinkedAsync(request.ProfessionId, request.SpeciesId, cancellationToken))
            {
                throw new DomainValidationException("This species is already linked to the profession.");
            }

            await _repository.AddSpeciesAsync(request.ProfessionId, request.SpeciesId, cancellationToken);
            return true;
        }
    }

    public class RemoveProfessionSpeciesCommandHandler : IRequestHandler<RemoveProfessionSpeciesCommand, bool>
    {
        private readonly IProfessionRepository _repository;

        public RemoveProfessionSpeciesCommandHandler(IProfessionRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(RemoveProfessionSpeciesCommand request, CancellationToken cancellationToken)
        {
            return await _repository.RemoveSpeciesAsync(request.ProfessionId, request.SpeciesId, cancellationToken);
        }
    }

    // ----- Social class link -----

    public class AddProfessionSocialClassCommandHandler : IRequestHandler<AddProfessionSocialClassCommand, bool>
    {
        private readonly IProfessionRepository _repository;
        private readonly ISocialClassRepository _socialClassRepository;

        public AddProfessionSocialClassCommandHandler(IProfessionRepository repository, ISocialClassRepository socialClassRepository)
        {
            _repository = repository;
            _socialClassRepository = socialClassRepository;
        }

        public async Task<bool> Handle(AddProfessionSocialClassCommand request, CancellationToken cancellationToken)
        {
            var profession = await _repository.FindByIdAsync(request.ProfessionId, cancellationToken)
                ?? throw new EntityNotFoundException("Profession", request.ProfessionId);

            var socialClass = await _socialClassRepository.FindByIdAsync(request.SocialClassId, cancellationToken)
                ?? throw new DomainValidationException($"Social class with ID {request.SocialClassId} does not exist.");
            if (socialClass.WorldId != profession.WorldId)
            {
                throw new DomainValidationException($"Social class with ID {request.SocialClassId} does not belong to this world.");
            }

            if (await _repository.IsSocialClassLinkedAsync(request.ProfessionId, request.SocialClassId, cancellationToken))
            {
                throw new DomainValidationException("This social class is already linked to the profession.");
            }

            await _repository.AddSocialClassAsync(request.ProfessionId, request.SocialClassId, cancellationToken);
            return true;
        }
    }

    public class RemoveProfessionSocialClassCommandHandler : IRequestHandler<RemoveProfessionSocialClassCommand, bool>
    {
        private readonly IProfessionRepository _repository;

        public RemoveProfessionSocialClassCommandHandler(IProfessionRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(RemoveProfessionSocialClassCommand request, CancellationToken cancellationToken)
        {
            return await _repository.RemoveSocialClassAsync(request.ProfessionId, request.SocialClassId, cancellationToken);
        }
    }

    // ----- Trade school link -----

    public class AddProfessionTradeSchoolCommandHandler : IRequestHandler<AddProfessionTradeSchoolCommand, bool>
    {
        private readonly IProfessionRepository _repository;
        private readonly ITradeSchoolRepository _tradeSchoolRepository;

        public AddProfessionTradeSchoolCommandHandler(IProfessionRepository repository, ITradeSchoolRepository tradeSchoolRepository)
        {
            _repository = repository;
            _tradeSchoolRepository = tradeSchoolRepository;
        }

        public async Task<bool> Handle(AddProfessionTradeSchoolCommand request, CancellationToken cancellationToken)
        {
            var profession = await _repository.FindByIdAsync(request.ProfessionId, cancellationToken)
                ?? throw new EntityNotFoundException("Profession", request.ProfessionId);

            var tradeSchool = await _tradeSchoolRepository.FindByIdAsync(request.TradeSchoolId, cancellationToken)
                ?? throw new DomainValidationException($"Trade school with ID {request.TradeSchoolId} does not exist.");
            if (tradeSchool.WorldId != profession.WorldId)
            {
                throw new DomainValidationException($"Trade school with ID {request.TradeSchoolId} does not belong to this world.");
            }

            if (await _repository.IsTradeSchoolLinkedAsync(request.ProfessionId, request.TradeSchoolId, cancellationToken))
            {
                throw new DomainValidationException("This trade school is already linked to the profession.");
            }

            await _repository.AddTradeSchoolAsync(request.ProfessionId, request.TradeSchoolId, cancellationToken);
            return true;
        }
    }

    public class RemoveProfessionTradeSchoolCommandHandler : IRequestHandler<RemoveProfessionTradeSchoolCommand, bool>
    {
        private readonly IProfessionRepository _repository;

        public RemoveProfessionTradeSchoolCommandHandler(IProfessionRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(RemoveProfessionTradeSchoolCommand request, CancellationToken cancellationToken)
        {
            return await _repository.RemoveTradeSchoolAsync(request.ProfessionId, request.TradeSchoolId, cancellationToken);
        }
    }
}
