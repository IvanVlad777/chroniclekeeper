using AutoMapper;
using ChronicleKeeper.Core.CQRS.ExtractionMethods.Commands;
using ChronicleKeeper.Core.CQRS.ExtractionMethods.Queries;
using ChronicleKeeper.Core.DTOs.ExtractionMethod;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.ExtractionMethods.Handlers
{
    public class GetAllExtractionMethodsQueryHandler : IRequestHandler<GetAllExtractionMethodsQuery, List<ExtractionMethodDto>>
    {
        private readonly IExtractionMethodRepository _repository;
        private readonly IMapper _mapper;

        public GetAllExtractionMethodsQueryHandler(IExtractionMethodRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ExtractionMethodDto>> Handle(GetAllExtractionMethodsQuery request, CancellationToken cancellationToken)
        {
            var extractionMethods = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<ExtractionMethodDto>>(extractionMethods);
        }
    }

    public class GetExtractionMethodByIdQueryHandler : IRequestHandler<GetExtractionMethodByIdQuery, ExtractionMethodDetailsDto?>
    {
        private readonly IExtractionMethodRepository _repository;
        private readonly IMapper _mapper;

        public GetExtractionMethodByIdQueryHandler(IExtractionMethodRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ExtractionMethodDetailsDto?> Handle(GetExtractionMethodByIdQuery request, CancellationToken cancellationToken)
        {
            var extractionMethod = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return extractionMethod == null ? null : _mapper.Map<ExtractionMethodDetailsDto>(extractionMethod);
        }
    }

    public class CreateExtractionMethodCommandHandler : IRequestHandler<CreateExtractionMethodCommand, ExtractionMethodDto>
    {
        private readonly IExtractionMethodRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateExtractionMethodCommandHandler> _logger;

        public CreateExtractionMethodCommandHandler(
            IExtractionMethodRepository repository,
            IWorldRepository worldRepository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<CreateExtractionMethodCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ExtractionMethodDto> Handle(CreateExtractionMethodCommand request, CancellationToken cancellationToken)
        {
            var dto = request.ExtractionMethodCreateDto;
            _logger.LogInformation("Creating new extraction method: {Name}", dto.Name);

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

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Social.Economy.ExtractionMethod>(dto), cancellationToken);
            return _mapper.Map<ExtractionMethodDto>(created);
        }
    }

    public class UpdateExtractionMethodCommandHandler : IRequestHandler<UpdateExtractionMethodCommand, ExtractionMethodDto>
    {
        private readonly IExtractionMethodRepository _repository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public UpdateExtractionMethodCommandHandler(
            IExtractionMethodRepository repository,
            IHistoryRepository historyRepository,
            IMapper mapper)
        {
            _repository = repository;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public async Task<ExtractionMethodDto> Handle(UpdateExtractionMethodCommand request, CancellationToken cancellationToken)
        {
            var extractionMethod = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("ExtractionMethod", request.Id);

            var dto = request.ExtractionMethodUpdateDto;

            if (dto.HistoryId is int historyId)
            {
                var history = await _historyRepository.FindByIdAsync(historyId, cancellationToken)
                    ?? throw new DomainValidationException($"History with ID {historyId} does not exist.");
                if (history.WorldId != extractionMethod.WorldId)
                {
                    throw new DomainValidationException($"History with ID {historyId} does not belong to this world.");
                }
            }

            _mapper.Map(dto, extractionMethod);
            var updated = await _repository.UpdateAsync(extractionMethod, cancellationToken);
            return _mapper.Map<ExtractionMethodDto>(updated);
        }
    }

    public class DeleteExtractionMethodCommandHandler : IRequestHandler<DeleteExtractionMethodCommand, bool>
    {
        private readonly IExtractionMethodRepository _repository;
        private readonly ILogger<DeleteExtractionMethodCommandHandler> _logger;

        public DeleteExtractionMethodCommandHandler(IExtractionMethodRepository repository, ILogger<DeleteExtractionMethodCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteExtractionMethodCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting extraction method with ID {Id}", request.Id);

            var resourcesInUse = await _repository.CountNaturalResourcesUsingExtractionMethodAsync(request.Id, cancellationToken);
            if (resourcesInUse > 0)
            {
                throw new DomainValidationException(
                    $"This extraction method is used by {resourcesInUse} natural resource(s). Reassign them first.");
            }

            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
