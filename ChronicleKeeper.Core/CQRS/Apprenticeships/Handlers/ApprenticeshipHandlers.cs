using AutoMapper;
using ChronicleKeeper.Core.CQRS.Apprenticeships.Commands;
using ChronicleKeeper.Core.CQRS.Apprenticeships.Queries;
using ChronicleKeeper.Core.DTOs.Apprenticeship;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Apprenticeships.Handlers
{
    public class GetAllApprenticeshipsQueryHandler : IRequestHandler<GetAllApprenticeshipsQuery, List<ApprenticeshipDto>>
    {
        private readonly IApprenticeshipRepository _repository;
        private readonly IMapper _mapper;

        public GetAllApprenticeshipsQueryHandler(IApprenticeshipRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ApprenticeshipDto>> Handle(GetAllApprenticeshipsQuery request, CancellationToken cancellationToken)
        {
            var apprenticeships = await _repository.GetAllAsync(request.WorldId, request.ProfessionId, cancellationToken);
            return _mapper.Map<List<ApprenticeshipDto>>(apprenticeships);
        }
    }

    public class GetApprenticeshipByIdQueryHandler : IRequestHandler<GetApprenticeshipByIdQuery, ApprenticeshipDto?>
    {
        private readonly IApprenticeshipRepository _repository;
        private readonly IMapper _mapper;

        public GetApprenticeshipByIdQueryHandler(IApprenticeshipRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApprenticeshipDto?> Handle(GetApprenticeshipByIdQuery request, CancellationToken cancellationToken)
        {
            var apprenticeship = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return apprenticeship == null ? null : _mapper.Map<ApprenticeshipDto>(apprenticeship);
        }
    }

    public class CreateApprenticeshipCommandHandler : IRequestHandler<CreateApprenticeshipCommand, ApprenticeshipDto>
    {
        private readonly IApprenticeshipRepository _repository;
        private readonly IProfessionRepository _professionRepository;
        private readonly ITradeSchoolRepository _tradeSchoolRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateApprenticeshipCommandHandler> _logger;

        public CreateApprenticeshipCommandHandler(
            IApprenticeshipRepository repository,
            IProfessionRepository professionRepository,
            ITradeSchoolRepository tradeSchoolRepository,
            IMapper mapper,
            ILogger<CreateApprenticeshipCommandHandler> logger)
        {
            _repository = repository;
            _professionRepository = professionRepository;
            _tradeSchoolRepository = tradeSchoolRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApprenticeshipDto> Handle(CreateApprenticeshipCommand request, CancellationToken cancellationToken)
        {
            var dto = request.ApprenticeshipCreateDto;
            _logger.LogInformation("Creating new apprenticeship: {Name}", dto.Name);

            var profession = await _professionRepository.FindByIdAsync(dto.ProfessionId, cancellationToken)
                ?? throw new DomainValidationException($"Profession with ID {dto.ProfessionId} does not exist.");

            if (dto.TradeSchoolId is int tradeSchoolId)
            {
                var tradeSchool = await _tradeSchoolRepository.FindByIdAsync(tradeSchoolId, cancellationToken)
                    ?? throw new DomainValidationException($"Trade school with ID {tradeSchoolId} does not exist.");
                if (tradeSchool.WorldId != profession.WorldId)
                {
                    throw new DomainValidationException($"Trade school with ID {tradeSchoolId} does not belong to this world.");
                }
            }

            var apprenticeship = _mapper.Map<Entities.Professions.Apprenticeship>(dto);
            apprenticeship.WorldId = profession.WorldId; // svijet naukovanja uvijek = svijet zanimanja

            var created = await _repository.CreateAsync(apprenticeship, cancellationToken);
            return _mapper.Map<ApprenticeshipDto>(created);
        }
    }

    public class UpdateApprenticeshipCommandHandler : IRequestHandler<UpdateApprenticeshipCommand, ApprenticeshipDto>
    {
        private readonly IApprenticeshipRepository _repository;
        private readonly ITradeSchoolRepository _tradeSchoolRepository;
        private readonly IMapper _mapper;

        public UpdateApprenticeshipCommandHandler(
            IApprenticeshipRepository repository,
            ITradeSchoolRepository tradeSchoolRepository,
            IMapper mapper)
        {
            _repository = repository;
            _tradeSchoolRepository = tradeSchoolRepository;
            _mapper = mapper;
        }

        public async Task<ApprenticeshipDto> Handle(UpdateApprenticeshipCommand request, CancellationToken cancellationToken)
        {
            var apprenticeship = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Apprenticeship", request.Id);

            var dto = request.ApprenticeshipUpdateDto;
            if (dto.TradeSchoolId is int tradeSchoolId)
            {
                var tradeSchool = await _tradeSchoolRepository.FindByIdAsync(tradeSchoolId, cancellationToken)
                    ?? throw new DomainValidationException($"Trade school with ID {tradeSchoolId} does not exist.");
                if (tradeSchool.WorldId != apprenticeship.WorldId)
                {
                    throw new DomainValidationException($"Trade school with ID {tradeSchoolId} does not belong to this world.");
                }
            }

            _mapper.Map(dto, apprenticeship);
            var updated = await _repository.UpdateAsync(apprenticeship, cancellationToken);
            return _mapper.Map<ApprenticeshipDto>(updated);
        }
    }

    public class DeleteApprenticeshipCommandHandler : IRequestHandler<DeleteApprenticeshipCommand, bool>
    {
        private readonly IApprenticeshipRepository _repository;
        private readonly ILogger<DeleteApprenticeshipCommandHandler> _logger;

        public DeleteApprenticeshipCommandHandler(IApprenticeshipRepository repository, ILogger<DeleteApprenticeshipCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteApprenticeshipCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting apprenticeship with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
