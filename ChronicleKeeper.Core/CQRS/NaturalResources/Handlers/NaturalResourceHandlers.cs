using AutoMapper;
using ChronicleKeeper.Core.CQRS.NaturalResources.Commands;
using ChronicleKeeper.Core.CQRS.NaturalResources.Queries;
using ChronicleKeeper.Core.DTOs.NaturalResource;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.NaturalResources.Handlers
{
    public class GetAllNaturalResourcesQueryHandler : IRequestHandler<GetAllNaturalResourcesQuery, List<NaturalResourceDto>>
    {
        private readonly INaturalResourceRepository _repository;
        private readonly IMapper _mapper;

        public GetAllNaturalResourcesQueryHandler(INaturalResourceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<NaturalResourceDto>> Handle(GetAllNaturalResourcesQuery request, CancellationToken cancellationToken)
        {
            var naturalResources = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<NaturalResourceDto>>(naturalResources);
        }
    }

    public class GetNaturalResourceByIdQueryHandler : IRequestHandler<GetNaturalResourceByIdQuery, NaturalResourceDetailsDto?>
    {
        private readonly INaturalResourceRepository _repository;
        private readonly IMapper _mapper;

        public GetNaturalResourceByIdQueryHandler(INaturalResourceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<NaturalResourceDetailsDto?> Handle(GetNaturalResourceByIdQuery request, CancellationToken cancellationToken)
        {
            var naturalResource = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return naturalResource == null ? null : _mapper.Map<NaturalResourceDetailsDto>(naturalResource);
        }
    }

    public class CreateNaturalResourceCommandHandler : IRequestHandler<CreateNaturalResourceCommand, NaturalResourceDto>
    {
        private readonly INaturalResourceRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IExtractionMethodRepository _extractionMethodRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateNaturalResourceCommandHandler> _logger;

        public CreateNaturalResourceCommandHandler(
            INaturalResourceRepository repository,
            IWorldRepository worldRepository,
            IExtractionMethodRepository extractionMethodRepository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<CreateNaturalResourceCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _extractionMethodRepository = extractionMethodRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<NaturalResourceDto> Handle(CreateNaturalResourceCommand request, CancellationToken cancellationToken)
        {
            var dto = request.NaturalResourceCreateDto;
            _logger.LogInformation("Creating new natural resource: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            await NaturalResourceValidation.ValidateReferencesAsync(
                dto.ExtractionMethodId, dto.HistoryId, dto.WorldId, _extractionMethodRepository, _historyRepository, cancellationToken);

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Social.Economy.NaturalResource>(dto), cancellationToken);
            return _mapper.Map<NaturalResourceDto>(created);
        }
    }

    public class UpdateNaturalResourceCommandHandler : IRequestHandler<UpdateNaturalResourceCommand, NaturalResourceDto>
    {
        private readonly INaturalResourceRepository _repository;
        private readonly IExtractionMethodRepository _extractionMethodRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public UpdateNaturalResourceCommandHandler(
            INaturalResourceRepository repository,
            IExtractionMethodRepository extractionMethodRepository,
            IHistoryRepository historyRepository,
            IMapper mapper)
        {
            _repository = repository;
            _extractionMethodRepository = extractionMethodRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public async Task<NaturalResourceDto> Handle(UpdateNaturalResourceCommand request, CancellationToken cancellationToken)
        {
            var naturalResource = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("NaturalResource", request.Id);

            var dto = request.NaturalResourceUpdateDto;

            await NaturalResourceValidation.ValidateReferencesAsync(
                dto.ExtractionMethodId, dto.HistoryId, naturalResource.WorldId, _extractionMethodRepository, _historyRepository, cancellationToken);

            _mapper.Map(dto, naturalResource);
            var updated = await _repository.UpdateAsync(naturalResource, cancellationToken);
            return _mapper.Map<NaturalResourceDto>(updated);
        }
    }

    public class DeleteNaturalResourceCommandHandler : IRequestHandler<DeleteNaturalResourceCommand, bool>
    {
        private readonly INaturalResourceRepository _repository;
        private readonly ILogger<DeleteNaturalResourceCommandHandler> _logger;

        public DeleteNaturalResourceCommandHandler(INaturalResourceRepository repository, ILogger<DeleteNaturalResourceCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteNaturalResourceCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting natural resource with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    public class AddNaturalResourceLocationCommandHandler : IRequestHandler<AddNaturalResourceLocationCommand, bool>
    {
        private readonly INaturalResourceRepository _repository;
        private readonly ILocationRepository _locationRepository;

        public AddNaturalResourceLocationCommandHandler(INaturalResourceRepository repository, ILocationRepository locationRepository)
        {
            _repository = repository;
            _locationRepository = locationRepository;
        }

        public async Task<bool> Handle(AddNaturalResourceLocationCommand request, CancellationToken cancellationToken)
        {
            var naturalResource = await _repository.FindByIdAsync(request.NaturalResourceId, cancellationToken)
                ?? throw new EntityNotFoundException("NaturalResource", request.NaturalResourceId);

            var location = await _locationRepository.FindByIdAsync(request.LocationId, cancellationToken)
                ?? throw new DomainValidationException($"Location with ID {request.LocationId} does not exist.");
            if (location.WorldId != naturalResource.WorldId)
            {
                throw new DomainValidationException($"Location with ID {request.LocationId} does not belong to this world.");
            }

            if (await _repository.IsLocationLinkedAsync(request.NaturalResourceId, request.LocationId, cancellationToken))
            {
                throw new DomainValidationException("This location is already linked to the natural resource.");
            }

            await _repository.AddLocationAsync(request.NaturalResourceId, request.LocationId, cancellationToken);
            return true;
        }
    }

    public class RemoveNaturalResourceLocationCommandHandler : IRequestHandler<RemoveNaturalResourceLocationCommand, bool>
    {
        private readonly INaturalResourceRepository _repository;

        public RemoveNaturalResourceLocationCommandHandler(INaturalResourceRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(RemoveNaturalResourceLocationCommand request, CancellationToken cancellationToken)
        {
            return _repository.RemoveLocationAsync(request.NaturalResourceId, request.LocationId, cancellationToken);
        }
    }

    internal static class NaturalResourceValidation
    {
        internal static async Task ValidateReferencesAsync(
            int? extractionMethodId,
            int? historyId,
            int worldId,
            IExtractionMethodRepository extractionMethodRepository,
            IHistoryRepository historyRepository,
            CancellationToken cancellationToken)
        {
            if (extractionMethodId is int eid)
            {
                var extractionMethod = await extractionMethodRepository.FindByIdAsync(eid, cancellationToken)
                    ?? throw new DomainValidationException($"Extraction method with ID {eid} does not exist.");
                if (extractionMethod.WorldId != worldId)
                {
                    throw new DomainValidationException($"Extraction method with ID {eid} does not belong to this world.");
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
