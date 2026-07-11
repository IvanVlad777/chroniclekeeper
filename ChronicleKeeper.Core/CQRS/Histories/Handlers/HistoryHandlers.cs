using AutoMapper;
using ChronicleKeeper.Core.CQRS.Histories.Commands;
using ChronicleKeeper.Core.CQRS.Histories.Queries;
using ChronicleKeeper.Core.DTOs.History;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Histories.Handlers
{
    public class GetAllHistoriesQueryHandler : IRequestHandler<GetAllHistoriesQuery, List<HistoryDto>>
    {
        private readonly IHistoryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllHistoriesQueryHandler(IHistoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<HistoryDto>> Handle(GetAllHistoriesQuery request, CancellationToken cancellationToken)
        {
            var histories = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<HistoryDto>>(histories);
        }
    }

    public class GetHistoryByIdQueryHandler : IRequestHandler<GetHistoryByIdQuery, HistoryDetailsDto?>
    {
        private readonly IHistoryRepository _repository;
        private readonly IMapper _mapper;

        public GetHistoryByIdQueryHandler(IHistoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<HistoryDetailsDto?> Handle(GetHistoryByIdQuery request, CancellationToken cancellationToken)
        {
            var history = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return history == null ? null : _mapper.Map<HistoryDetailsDto>(history);
        }
    }

    public class CreateHistoryCommandHandler : IRequestHandler<CreateHistoryCommand, HistoryDto>
    {
        private readonly IHistoryRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateHistoryCommandHandler> _logger;

        public CreateHistoryCommandHandler(
            IHistoryRepository repository,
            IWorldRepository worldRepository,
            IMapper mapper,
            ILogger<CreateHistoryCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<HistoryDto> Handle(CreateHistoryCommand request, CancellationToken cancellationToken)
        {
            var dto = request.HistoryCreateDto;
            _logger.LogInformation("Creating new history: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            var history = _mapper.Map<Entities.HistoryTimelines.History>(dto);
            var created = await _repository.CreateAsync(history, cancellationToken);
            return _mapper.Map<HistoryDto>(created);
        }
    }

    public class UpdateHistoryCommandHandler : IRequestHandler<UpdateHistoryCommand, HistoryDto>
    {
        private readonly IHistoryRepository _repository;
        private readonly IMapper _mapper;

        public UpdateHistoryCommandHandler(IHistoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<HistoryDto> Handle(UpdateHistoryCommand request, CancellationToken cancellationToken)
        {
            var history = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("History", request.Id);

            _mapper.Map(request.HistoryUpdateDto, history);
            var updated = await _repository.UpdateAsync(history, cancellationToken);
            return _mapper.Map<HistoryDto>(updated);
        }
    }

    public class DeleteHistoryCommandHandler : IRequestHandler<DeleteHistoryCommand, bool>
    {
        private readonly IHistoryRepository _repository;
        private readonly ILogger<DeleteHistoryCommandHandler> _logger;

        public DeleteHistoryCommandHandler(IHistoryRepository repository, ILogger<DeleteHistoryCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteHistoryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting history with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
