using AutoMapper;
using ChronicleKeeper.Core.CQRS.ClimateDetails.Commands;
using ChronicleKeeper.Core.CQRS.ClimateDetails.Queries;
using ChronicleKeeper.Core.DTOs.ClimateDetail;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.ClimateDetails.Handlers
{
    public class GetAllClimateDetailsQueryHandler : IRequestHandler<GetAllClimateDetailsQuery, List<ClimateDetailDto>>
    {
        private readonly IClimateDetailRepository _repository;
        private readonly IMapper _mapper;

        public GetAllClimateDetailsQueryHandler(IClimateDetailRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ClimateDetailDto>> Handle(GetAllClimateDetailsQuery request, CancellationToken cancellationToken)
        {
            var climateDetails = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<ClimateDetailDto>>(climateDetails);
        }
    }

    public class GetClimateDetailByIdQueryHandler : IRequestHandler<GetClimateDetailByIdQuery, ClimateDetailDto?>
    {
        private readonly IClimateDetailRepository _repository;
        private readonly IMapper _mapper;

        public GetClimateDetailByIdQueryHandler(IClimateDetailRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ClimateDetailDto?> Handle(GetClimateDetailByIdQuery request, CancellationToken cancellationToken)
        {
            var climateDetail = await _repository.FindByIdAsync(request.Id, cancellationToken);
            return climateDetail == null ? null : _mapper.Map<ClimateDetailDto>(climateDetail);
        }
    }

    public class CreateClimateDetailCommandHandler : IRequestHandler<CreateClimateDetailCommand, ClimateDetailDto>
    {
        private readonly IClimateDetailRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateClimateDetailCommandHandler> _logger;

        public CreateClimateDetailCommandHandler(
            IClimateDetailRepository repository,
            IWorldRepository worldRepository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<CreateClimateDetailCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ClimateDetailDto> Handle(CreateClimateDetailCommand request, CancellationToken cancellationToken)
        {
            var dto = request.ClimateDetailCreateDto;
            _logger.LogInformation("Creating new climate detail: {Name}", dto.Name);

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

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Geography.Climate.ClimateDetail>(dto), cancellationToken);
            return _mapper.Map<ClimateDetailDto>(created);
        }
    }

    public class UpdateClimateDetailCommandHandler : IRequestHandler<UpdateClimateDetailCommand, ClimateDetailDto>
    {
        private readonly IClimateDetailRepository _repository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public UpdateClimateDetailCommandHandler(
            IClimateDetailRepository repository,
            IHistoryRepository historyRepository,
            IMapper mapper)
        {
            _repository = repository;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public async Task<ClimateDetailDto> Handle(UpdateClimateDetailCommand request, CancellationToken cancellationToken)
        {
            var climateDetail = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("ClimateDetail", request.Id);

            var dto = request.ClimateDetailUpdateDto;

            if (dto.HistoryId is int historyId)
            {
                var history = await _historyRepository.FindByIdAsync(historyId, cancellationToken)
                    ?? throw new DomainValidationException($"History with ID {historyId} does not exist.");
                if (history.WorldId != climateDetail.WorldId)
                {
                    throw new DomainValidationException($"History with ID {historyId} does not belong to this world.");
                }
            }

            _mapper.Map(dto, climateDetail);
            var updated = await _repository.UpdateAsync(climateDetail, cancellationToken);
            return _mapper.Map<ClimateDetailDto>(updated);
        }
    }

    public class DeleteClimateDetailCommandHandler : IRequestHandler<DeleteClimateDetailCommand, bool>
    {
        private readonly IClimateDetailRepository _repository;
        private readonly ILogger<DeleteClimateDetailCommandHandler> _logger;

        public DeleteClimateDetailCommandHandler(IClimateDetailRepository repository, ILogger<DeleteClimateDetailCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteClimateDetailCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting climate detail with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
