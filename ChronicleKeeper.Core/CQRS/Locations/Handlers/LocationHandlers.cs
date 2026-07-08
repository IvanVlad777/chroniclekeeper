using AutoMapper;
using ChronicleKeeper.Core.CQRS.Locations.Commands;
using ChronicleKeeper.Core.CQRS.Locations.Queries;
using ChronicleKeeper.Core.DTOs.Location;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Locations.Handlers
{
    public class GetAllLocationsQueryHandler : IRequestHandler<GetAllLocationsQuery, List<LocationDto>>
    {
        private readonly ILocationRepository _repository;
        private readonly IMapper _mapper;

        public GetAllLocationsQueryHandler(ILocationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<LocationDto>> Handle(GetAllLocationsQuery request, CancellationToken cancellationToken)
        {
            var locations = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<LocationDto>>(locations);
        }
    }

    public class GetLocationByIdQueryHandler : IRequestHandler<GetLocationByIdQuery, LocationDetailsDto?>
    {
        private readonly ILocationRepository _repository;
        private readonly IMapper _mapper;

        public GetLocationByIdQueryHandler(ILocationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<LocationDetailsDto?> Handle(GetLocationByIdQuery request, CancellationToken cancellationToken)
        {
            var location = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return location == null ? null : _mapper.Map<LocationDetailsDto>(location);
        }
    }

    public class CreateLocationCommandHandler : IRequestHandler<CreateLocationCommand, LocationDto>
    {
        private readonly ILocationRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateLocationCommandHandler> _logger;

        public CreateLocationCommandHandler(
            ILocationRepository repository,
            IWorldRepository worldRepository,
            IMapper mapper,
            ILogger<CreateLocationCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LocationDto> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
        {
            var dto = request.LocationCreateDto;
            _logger.LogInformation("Creating new location: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            var location = _mapper.Map<Location>(dto);
            await LocationValidation.ValidateParentAsync(_repository, location, cancellationToken);

            var created = await _repository.CreateAsync(location, cancellationToken);
            _logger.LogInformation("Created location with ID {Id}", created.Id);

            return _mapper.Map<LocationDto>(created);
        }
    }

    public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, LocationDto>
    {
        private readonly ILocationRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateLocationCommandHandler> _logger;

        public UpdateLocationCommandHandler(
            ILocationRepository repository,
            IMapper mapper,
            ILogger<UpdateLocationCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LocationDto> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating location with ID {Id}", request.Id);

            var location = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Location", request.Id);

            _mapper.Map(request.LocationUpdateDto, location);
            await LocationValidation.ValidateParentAsync(_repository, location, cancellationToken);

            var updated = await _repository.UpdateAsync(location, cancellationToken);
            return _mapper.Map<LocationDto>(updated);
        }
    }

    public class DeleteLocationCommandHandler : IRequestHandler<DeleteLocationCommand, bool>
    {
        private readonly ILocationRepository _repository;
        private readonly ILogger<DeleteLocationCommandHandler> _logger;

        public DeleteLocationCommandHandler(ILocationRepository repository, ILogger<DeleteLocationCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteLocationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting location with ID {Id}", request.Id);

            if (await _repository.HasChildrenAsync(request.Id, cancellationToken))
            {
                throw new DomainValidationException(
                    "This location has sub-locations. Reparent or delete them first.");
            }

            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    internal static class LocationValidation
    {
        /// <summary>Roditelj mora postojati u istom svijetu, ne smije biti sama lokacija niti stvarati ciklus.</summary>
        public static async Task ValidateParentAsync(
            ILocationRepository repository, Location location, CancellationToken cancellationToken)
        {
            if (location.ParentLocationId is not int parentId) return;

            if (parentId == location.Id)
            {
                throw new DomainValidationException("A location cannot be its own parent.");
            }
            if (!await repository.ExistsInWorldAsync(parentId, location.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"Parent location with ID {parentId} does not exist in this world.");
            }
            // Ciklus je moguć samo za postojeću lokaciju (novokreirana još nema djece)
            if (location.Id != 0 && await repository.WouldCreateCycleAsync(location.Id, parentId, cancellationToken))
            {
                throw new DomainValidationException("This parent assignment would create a cycle in the location hierarchy.");
            }
        }
    }
}
