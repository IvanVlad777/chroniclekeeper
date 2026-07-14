using AutoMapper;
using ChronicleKeeper.Core.CQRS.Industries.Commands;
using ChronicleKeeper.Core.CQRS.Industries.Queries;
using ChronicleKeeper.Core.DTOs.Industry;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Industries.Handlers
{
    public class GetAllIndustriesQueryHandler : IRequestHandler<GetAllIndustriesQuery, List<IndustryDto>>
    {
        private readonly IIndustryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllIndustriesQueryHandler(IIndustryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<IndustryDto>> Handle(GetAllIndustriesQuery request, CancellationToken cancellationToken)
        {
            var industries = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<IndustryDto>>(industries);
        }
    }

    public class GetIndustryByIdQueryHandler : IRequestHandler<GetIndustryByIdQuery, IndustryDetailsDto?>
    {
        private readonly IIndustryRepository _repository;
        private readonly IMapper _mapper;

        public GetIndustryByIdQueryHandler(IIndustryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IndustryDetailsDto?> Handle(GetIndustryByIdQuery request, CancellationToken cancellationToken)
        {
            var industry = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return industry == null ? null : _mapper.Map<IndustryDetailsDto>(industry);
        }
    }

    public class CreateIndustryCommandHandler : IRequestHandler<CreateIndustryCommand, IndustryDto>
    {
        private readonly IIndustryRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateIndustryCommandHandler> _logger;

        public CreateIndustryCommandHandler(
            IIndustryRepository repository,
            IWorldRepository worldRepository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<CreateIndustryCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IndustryDto> Handle(CreateIndustryCommand request, CancellationToken cancellationToken)
        {
            var dto = request.IndustryCreateDto;
            _logger.LogInformation("Creating new industry: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            if (dto.HistoryId is int historyId)
            {
                var history = await _historyRepository.FindByIdAsync(historyId, cancellationToken)
                    ?? throw new DomainValidationException($"History with ID {historyId} does not exist.");
                if (history.WorldId != dto.WorldId)
                {
                    throw new DomainValidationException($"History with ID {historyId} does not belong to this world.");
                }
            }

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Social.Economy.Industry>(dto), cancellationToken);
            return _mapper.Map<IndustryDto>(created);
        }
    }

    public class UpdateIndustryCommandHandler : IRequestHandler<UpdateIndustryCommand, IndustryDto>
    {
        private readonly IIndustryRepository _repository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public UpdateIndustryCommandHandler(
            IIndustryRepository repository,
            IHistoryRepository historyRepository,
            IMapper mapper)
        {
            _repository = repository;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public async Task<IndustryDto> Handle(UpdateIndustryCommand request, CancellationToken cancellationToken)
        {
            var industry = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Industry", request.Id);

            var dto = request.IndustryUpdateDto;

            if (dto.HistoryId is int historyId)
            {
                var history = await _historyRepository.FindByIdAsync(historyId, cancellationToken)
                    ?? throw new DomainValidationException($"History with ID {historyId} does not exist.");
                if (history.WorldId != industry.WorldId)
                {
                    throw new DomainValidationException($"History with ID {historyId} does not belong to this world.");
                }
            }

            _mapper.Map(dto, industry);
            var updated = await _repository.UpdateAsync(industry, cancellationToken);
            return _mapper.Map<IndustryDto>(updated);
        }
    }

    public class DeleteIndustryCommandHandler : IRequestHandler<DeleteIndustryCommand, bool>
    {
        private readonly IIndustryRepository _repository;
        private readonly ILogger<DeleteIndustryCommandHandler> _logger;

        public DeleteIndustryCommandHandler(IIndustryRepository repository, ILogger<DeleteIndustryCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteIndustryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting industry with ID {Id}", request.Id);

            var guildsInUse = await _repository.CountGuildsUsingIndustryAsync(request.Id, cancellationToken);
            if (guildsInUse > 0)
            {
                throw new DomainValidationException(
                    $"This industry is used by {guildsInUse} guild(s). Reassign them first.");
            }

            var corporationsInUse = await _repository.CountCorporationsUsingIndustryAsync(request.Id, cancellationToken);
            if (corporationsInUse > 0)
            {
                throw new DomainValidationException(
                    $"This industry is used by {corporationsInUse} corporation(s). Reassign them first.");
            }

            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
