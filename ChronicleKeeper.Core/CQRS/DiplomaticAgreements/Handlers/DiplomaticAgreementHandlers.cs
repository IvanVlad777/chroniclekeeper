using AutoMapper;
using ChronicleKeeper.Core.CQRS.DiplomaticAgreements.Commands;
using ChronicleKeeper.Core.CQRS.DiplomaticAgreements.Queries;
using ChronicleKeeper.Core.DTOs.DiplomaticAgreement;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.DiplomaticAgreements.Handlers
{
    public class GetAllDiplomaticAgreementsQueryHandler : IRequestHandler<GetAllDiplomaticAgreementsQuery, List<DiplomaticAgreementDto>>
    {
        private readonly IDiplomaticAgreementRepository _repository;
        private readonly IMapper _mapper;

        public GetAllDiplomaticAgreementsQueryHandler(IDiplomaticAgreementRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<DiplomaticAgreementDto>> Handle(GetAllDiplomaticAgreementsQuery request, CancellationToken cancellationToken)
        {
            var agreements = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<DiplomaticAgreementDto>>(agreements);
        }
    }

    public class GetDiplomaticAgreementByIdQueryHandler : IRequestHandler<GetDiplomaticAgreementByIdQuery, DiplomaticAgreementDetailsDto?>
    {
        private readonly IDiplomaticAgreementRepository _repository;
        private readonly IMapper _mapper;

        public GetDiplomaticAgreementByIdQueryHandler(IDiplomaticAgreementRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<DiplomaticAgreementDetailsDto?> Handle(GetDiplomaticAgreementByIdQuery request, CancellationToken cancellationToken)
        {
            var agreement = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return agreement == null ? null : _mapper.Map<DiplomaticAgreementDetailsDto>(agreement);
        }
    }

    public class CreateDiplomaticAgreementCommandHandler : IRequestHandler<CreateDiplomaticAgreementCommand, DiplomaticAgreementDto>
    {
        private readonly IDiplomaticAgreementRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly INationRepository _nationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateDiplomaticAgreementCommandHandler> _logger;

        public CreateDiplomaticAgreementCommandHandler(
            IDiplomaticAgreementRepository repository,
            IWorldRepository worldRepository,
            INationRepository nationRepository,
            IMapper mapper,
            ILogger<CreateDiplomaticAgreementCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _nationRepository = nationRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DiplomaticAgreementDto> Handle(CreateDiplomaticAgreementCommand request, CancellationToken cancellationToken)
        {
            var dto = request.DiplomaticAgreementCreateDto;
            _logger.LogInformation("Creating new diplomatic agreement: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            if (dto.FirstNationId == dto.SecondNationId)
            {
                throw new DomainValidationException("A diplomatic agreement requires two different nations.");
            }

            var firstNation = await _nationRepository.FindByIdAsync(dto.FirstNationId, cancellationToken)
                ?? throw new DomainValidationException($"Nation with ID {dto.FirstNationId} does not exist.");
            if (firstNation.WorldId != dto.WorldId)
            {
                throw new DomainValidationException($"Nation with ID {dto.FirstNationId} does not belong to this world.");
            }

            var secondNation = await _nationRepository.FindByIdAsync(dto.SecondNationId, cancellationToken)
                ?? throw new DomainValidationException($"Nation with ID {dto.SecondNationId} does not exist.");
            if (secondNation.WorldId != dto.WorldId)
            {
                throw new DomainValidationException($"Nation with ID {dto.SecondNationId} does not belong to this world.");
            }

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Social.Politics.DiplomaticAgreement>(dto), cancellationToken);
            return _mapper.Map<DiplomaticAgreementDto>(created);
        }
    }

    public class UpdateDiplomaticAgreementCommandHandler : IRequestHandler<UpdateDiplomaticAgreementCommand, DiplomaticAgreementDto>
    {
        private readonly IDiplomaticAgreementRepository _repository;
        private readonly INationRepository _nationRepository;
        private readonly IMapper _mapper;

        public UpdateDiplomaticAgreementCommandHandler(
            IDiplomaticAgreementRepository repository,
            INationRepository nationRepository,
            IMapper mapper)
        {
            _repository = repository;
            _nationRepository = nationRepository;
            _mapper = mapper;
        }

        public async Task<DiplomaticAgreementDto> Handle(UpdateDiplomaticAgreementCommand request, CancellationToken cancellationToken)
        {
            var agreement = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("DiplomaticAgreement", request.Id);

            var dto = request.DiplomaticAgreementUpdateDto;

            if (dto.FirstNationId == dto.SecondNationId)
            {
                throw new DomainValidationException("A diplomatic agreement requires two different nations.");
            }

            var firstNation = await _nationRepository.FindByIdAsync(dto.FirstNationId, cancellationToken)
                ?? throw new DomainValidationException($"Nation with ID {dto.FirstNationId} does not exist.");
            if (firstNation.WorldId != agreement.WorldId)
            {
                throw new DomainValidationException($"Nation with ID {dto.FirstNationId} does not belong to this world.");
            }

            var secondNation = await _nationRepository.FindByIdAsync(dto.SecondNationId, cancellationToken)
                ?? throw new DomainValidationException($"Nation with ID {dto.SecondNationId} does not exist.");
            if (secondNation.WorldId != agreement.WorldId)
            {
                throw new DomainValidationException($"Nation with ID {dto.SecondNationId} does not belong to this world.");
            }

            _mapper.Map(dto, agreement);
            var updated = await _repository.UpdateAsync(agreement, cancellationToken);
            return _mapper.Map<DiplomaticAgreementDto>(updated);
        }
    }

    public class DeleteDiplomaticAgreementCommandHandler : IRequestHandler<DeleteDiplomaticAgreementCommand, bool>
    {
        private readonly IDiplomaticAgreementRepository _repository;
        private readonly ILogger<DeleteDiplomaticAgreementCommandHandler> _logger;

        public DeleteDiplomaticAgreementCommandHandler(IDiplomaticAgreementRepository repository, ILogger<DeleteDiplomaticAgreementCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteDiplomaticAgreementCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting diplomatic agreement with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
