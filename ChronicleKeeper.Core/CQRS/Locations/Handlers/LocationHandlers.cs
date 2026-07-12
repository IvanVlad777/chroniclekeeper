using AutoMapper;
using ChronicleKeeper.Core.CQRS.Locations.Commands;
using ChronicleKeeper.Core.CQRS.Locations.Queries;
using ChronicleKeeper.Core.DTOs.Location;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.Geography.Ecosystems;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using static ChronicleKeeper.Core.Enums.LoreEnums;

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
        private readonly IHistoryRepository _historyRepository;
        private readonly IGovernmentSystemRepository _governmentSystemRepository;
        private readonly ILegalSystemRepository _legalSystemRepository;
        private readonly IEducationSystemRepository _educationSystemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateLocationCommandHandler> _logger;

        public CreateLocationCommandHandler(
            ILocationRepository repository,
            IWorldRepository worldRepository,
            IHistoryRepository historyRepository,
            IGovernmentSystemRepository governmentSystemRepository,
            ILegalSystemRepository legalSystemRepository,
            IEducationSystemRepository educationSystemRepository,
            IMapper mapper,
            ILogger<CreateLocationCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _historyRepository = historyRepository;
            _governmentSystemRepository = governmentSystemRepository;
            _legalSystemRepository = legalSystemRepository;
            _educationSystemRepository = educationSystemRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LocationDto> Handle(CreateLocationCommand request, CancellationToken cancellationToken)
        {
            var dto = request.LocationCreateDto;
            _logger.LogInformation("Creating new location: {Name} ({Type})", dto.Name, dto.Type);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            Location location = dto.Type switch
            {
                LocationType.Continent => new Continent { ContinentSpecifics = dto.ContinentSpecifics },
                LocationType.Region => new Region { RegionSpecifics = dto.RegionSpecifics },
                LocationType.Country => new Country
                {
                    GovernmentSystemId = dto.GovernmentSystemId,
                    LegalSystemId = dto.LegalSystemId,
                    EducationSystemId = dto.EducationSystemId
                },
                LocationType.City => new City
                {
                    IsCapital = dto.IsCapital ?? false,
                    GovernmentSystemId = dto.GovernmentSystemId,
                    LegalSystemId = dto.LegalSystemId,
                    EducationSystemId = dto.EducationSystemId
                },
                LocationType.District => new District { DistrictType = dto.DistrictType ?? string.Empty },
                LocationType.Lake => new LakeEcosystem
                {
                    WaterDepth = dto.WaterDepth ?? 0,
                    Volume = dto.Volume ?? 0,
                    MaxDepth = dto.MaxDepth ?? 0,
                    IsFreshwater = dto.IsFreshwater ?? true
                },
                LocationType.Sea => new SeaEcosystem { WaterDepth = dto.WaterDepth ?? 0 },
                LocationType.Ocean => new OceanEcosystem { WaterDepth = dto.WaterDepth ?? 0 },
                LocationType.River => new RiverEcosystem
                {
                    WaterDepth = dto.WaterDepth ?? 0,
                    RiverLength = dto.RiverLength ?? 0,
                    SourceLocationId = dto.SourceLocationId,
                    MouthLocationId = dto.MouthLocationId
                },
                LocationType.Mountain => new MountainEcosystem
                {
                    MaxElevation = dto.MaxElevation ?? 0,
                    Prominence = dto.Prominence ?? 0
                },
                LocationType.MountainRange => new MountainRange { MountainRangeLength = dto.MountainRangeLength ?? 0 },
                LocationType.Swamp => new SwampEcosystem { IsSaltwater = dto.IsSaltwater ?? false },
                LocationType.Desert => new DesertEcosystem { DesertKind = dto.DesertKind ?? default },
                LocationType.Forest => new ForestEcosystem { ForestKind = dto.ForestKind ?? default },
                LocationType.Cave => new CaveEcosystem { CaveDepth = dto.CaveDepth ?? 0, CaveKind = dto.CaveKind ?? default },
                LocationType.Grassland => new GrasslandEcosystem { GrasslandKind = dto.GrasslandKind ?? default },
                _ => new Location()
            };

            if (location is Ecosystem ecosystem)
            {
                ecosystem.UniqueFeatures = dto.UniqueFeatures ?? string.Empty;
            }

            location.Name = dto.Name;
            location.Description = dto.Description;
            location.WorldId = dto.WorldId;
            location.Type = dto.Type;
            location.Area = dto.Area;
            location.Population = dto.Population;
            location.Latitude = dto.Latitude;
            location.Longitude = dto.Longitude;
            location.ParentLocationId = dto.ParentLocationId;
            location.HistoryId = dto.HistoryId;

            await LocationValidation.ValidateParentAsync(_repository, location, cancellationToken);
            await LocationValidation.ValidateHistoryAsync(_historyRepository, location, cancellationToken);
            await LocationValidation.ValidateSystemsAsync(
                _governmentSystemRepository, _legalSystemRepository, _educationSystemRepository, location, cancellationToken);
            await LocationValidation.ValidateRiverEndpointsAsync(_repository, location, cancellationToken);

            var created = await _repository.CreateAsync(location, cancellationToken);
            _logger.LogInformation("Created location with ID {Id}", created.Id);

            return _mapper.Map<LocationDto>(created);
        }
    }

    public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, LocationDto>
    {
        private readonly ILocationRepository _repository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IGovernmentSystemRepository _governmentSystemRepository;
        private readonly ILegalSystemRepository _legalSystemRepository;
        private readonly IEducationSystemRepository _educationSystemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateLocationCommandHandler> _logger;

        public UpdateLocationCommandHandler(
            ILocationRepository repository,
            IHistoryRepository historyRepository,
            IGovernmentSystemRepository governmentSystemRepository,
            ILegalSystemRepository legalSystemRepository,
            IEducationSystemRepository educationSystemRepository,
            IMapper mapper,
            ILogger<UpdateLocationCommandHandler> logger)
        {
            _repository = repository;
            _historyRepository = historyRepository;
            _governmentSystemRepository = governmentSystemRepository;
            _legalSystemRepository = legalSystemRepository;
            _educationSystemRepository = educationSystemRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LocationDto> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating location with ID {Id}", request.Id);

            var location = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Location", request.Id);

            var dto = request.LocationUpdateDto;

            if (LocationValidation.DiscriminatorGroup(dto.Type) != LocationValidation.DiscriminatorGroup(location.Type))
            {
                throw new DomainValidationException(
                    $"Changing a location's type from {location.Type} to {dto.Type} is not supported once created — create a new location instead.");
            }

            location.Name = dto.Name;
            location.Description = dto.Description;
            location.Type = dto.Type;
            location.Area = dto.Area;
            location.Population = dto.Population;
            location.Latitude = dto.Latitude;
            location.Longitude = dto.Longitude;
            location.ParentLocationId = dto.ParentLocationId;
            location.HistoryId = dto.HistoryId;

            switch (location)
            {
                case Continent continent:
                    continent.ContinentSpecifics = dto.ContinentSpecifics;
                    break;
                case Region region:
                    region.RegionSpecifics = dto.RegionSpecifics;
                    break;
                case Country country:
                    country.GovernmentSystemId = dto.GovernmentSystemId;
                    country.LegalSystemId = dto.LegalSystemId;
                    country.EducationSystemId = dto.EducationSystemId;
                    break;
                case City city:
                    city.IsCapital = dto.IsCapital ?? false;
                    city.GovernmentSystemId = dto.GovernmentSystemId;
                    city.LegalSystemId = dto.LegalSystemId;
                    city.EducationSystemId = dto.EducationSystemId;
                    break;
                case District district:
                    district.DistrictType = dto.DistrictType ?? string.Empty;
                    break;
                case LakeEcosystem lake:
                    lake.WaterDepth = dto.WaterDepth ?? 0;
                    lake.Volume = dto.Volume ?? 0;
                    lake.MaxDepth = dto.MaxDepth ?? 0;
                    lake.IsFreshwater = dto.IsFreshwater ?? true;
                    break;
                case RiverEcosystem river:
                    river.WaterDepth = dto.WaterDepth ?? 0;
                    river.RiverLength = dto.RiverLength ?? 0;
                    river.SourceLocationId = dto.SourceLocationId;
                    river.MouthLocationId = dto.MouthLocationId;
                    break;
                case MountainEcosystem mountain:
                    mountain.MaxElevation = dto.MaxElevation ?? 0;
                    mountain.Prominence = dto.Prominence ?? 0;
                    break;
                case MountainRange mountainRange:
                    mountainRange.MountainRangeLength = dto.MountainRangeLength ?? 0;
                    break;
                case SwampEcosystem swamp:
                    swamp.IsSaltwater = dto.IsSaltwater ?? false;
                    break;
                case DesertEcosystem desert:
                    desert.DesertKind = dto.DesertKind ?? default;
                    break;
                case ForestEcosystem forest:
                    forest.ForestKind = dto.ForestKind ?? default;
                    break;
                case CaveEcosystem cave:
                    cave.CaveDepth = dto.CaveDepth ?? 0;
                    cave.CaveKind = dto.CaveKind ?? default;
                    break;
                case GrasslandEcosystem grassland:
                    grassland.GrasslandKind = dto.GrasslandKind ?? default;
                    break;
                // SeaEcosystem/OceanEcosystem have no own fields beyond WaterEcosystem.WaterDepth.
                case WaterEcosystem water:
                    water.WaterDepth = dto.WaterDepth ?? 0;
                    break;
            }

            if (location is Ecosystem ecosystemUpdate)
            {
                ecosystemUpdate.UniqueFeatures = dto.UniqueFeatures ?? string.Empty;
            }

            await LocationValidation.ValidateParentAsync(_repository, location, cancellationToken);
            await LocationValidation.ValidateHistoryAsync(_historyRepository, location, cancellationToken);
            await LocationValidation.ValidateSystemsAsync(
                _governmentSystemRepository, _legalSystemRepository, _educationSystemRepository, location, cancellationToken);
            await LocationValidation.ValidateRiverEndpointsAsync(_repository, location, cancellationToken);

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

            if (await _repository.IsReferencedAsRiverEndpointAsync(request.Id, cancellationToken))
            {
                throw new DomainValidationException(
                    "This location is set as a river's source or mouth. Clear that reference first.");
            }

            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    public class AddRegionNativeSpeciesCommandHandler : IRequestHandler<AddRegionNativeSpeciesCommand, bool>
    {
        private readonly ILocationRepository _repository;
        private readonly ISpeciesRepository _speciesRepository;

        public AddRegionNativeSpeciesCommandHandler(ILocationRepository repository, ISpeciesRepository speciesRepository)
        {
            _repository = repository;
            _speciesRepository = speciesRepository;
        }

        public async Task<bool> Handle(AddRegionNativeSpeciesCommand request, CancellationToken cancellationToken)
        {
            var region = await _repository.FindByIdAsync(request.RegionId, cancellationToken)
                ?? throw new EntityNotFoundException("Region", request.RegionId);
            if (region is not Region)
            {
                throw new DomainValidationException($"Location with ID {request.RegionId} is not a Region.");
            }

            var species = await _speciesRepository.FindByIdAsync(request.SapientSpeciesId, cancellationToken)
                ?? throw new DomainValidationException($"Species with ID {request.SapientSpeciesId} does not exist.");
            if (species.WorldId != region.WorldId)
            {
                throw new DomainValidationException($"Species with ID {request.SapientSpeciesId} does not belong to this world.");
            }

            if (await _repository.IsNativeSpeciesLinkedAsync(request.RegionId, request.SapientSpeciesId, cancellationToken))
            {
                throw new DomainValidationException("This species is already linked to the region.");
            }

            await _repository.AddNativeSpeciesAsync(request.RegionId, request.SapientSpeciesId, cancellationToken);
            return true;
        }
    }

    public class RemoveRegionNativeSpeciesCommandHandler : IRequestHandler<RemoveRegionNativeSpeciesCommand, bool>
    {
        private readonly ILocationRepository _repository;

        public RemoveRegionNativeSpeciesCommandHandler(ILocationRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(RemoveRegionNativeSpeciesCommand request, CancellationToken cancellationToken)
        {
            return _repository.RemoveNativeSpeciesAsync(request.RegionId, request.SapientSpeciesId, cancellationToken);
        }
    }

    internal static class LocationValidation
    {
        /// <summary>Parent types allowed under each hierarchy subtype — Continent and the
        /// plain types (Town/Village/Building/Landmark/Wilderness/Other) accept any parent,
        /// matching today's behavior.</summary>
        private static readonly Dictionary<LocationType, LocationType[]> AllowedParentTypes = new()
        {
            [LocationType.District] = new[] { LocationType.City },
            [LocationType.City] = new[] { LocationType.Country, LocationType.Region },
            [LocationType.Country] = new[] { LocationType.Region, LocationType.Continent },
            [LocationType.Region] = new[] { LocationType.Continent },
            // Ecosystem sibling-pair containment (rides on ParentLocationId too) — the rest of the
            // Ecosystem subtypes (Lake/River/Swamp/Desert/Forest/Cave/Grassland/MountainRange) accept
            // any parent, same as Continent and the plain types.
            [LocationType.Sea] = new[] { LocationType.Ocean },
            [LocationType.Mountain] = new[] { LocationType.MountainRange },
        };

        /// <summary>Roditelj mora postojati u istom svijetu, ne smije biti sama lokacija, ne smije stvarati ciklus,
        /// i za hijerarhijske podtipove mora biti očekivanog tipa (npr. District samo pod City).</summary>
        public static async Task ValidateParentAsync(
            ILocationRepository repository, Location location, CancellationToken cancellationToken)
        {
            if (location.ParentLocationId is not int parentId) return;

            if (parentId == location.Id)
            {
                throw new DomainValidationException("A location cannot be its own parent.");
            }
            var parent = await repository.FindByIdAsync(parentId, cancellationToken);
            if (parent == null || parent.WorldId != location.WorldId)
            {
                throw new DomainValidationException($"Parent location with ID {parentId} does not exist in this world.");
            }
            // Ciklus je moguć samo za postojeću lokaciju (novokreirana još nema djece)
            if (location.Id != 0 && await repository.WouldCreateCycleAsync(location.Id, parentId, cancellationToken))
            {
                throw new DomainValidationException("This parent assignment would create a cycle in the location hierarchy.");
            }

            if (AllowedParentTypes.TryGetValue(location.Type, out var allowed) && !allowed.Contains(parent.Type))
            {
                throw new DomainValidationException(
                    $"A {location.Type} location cannot be parented under a {parent.Type} location.");
            }
        }

        public static async Task ValidateHistoryAsync(
            IHistoryRepository historyRepository, Location location, CancellationToken cancellationToken)
        {
            if (location.HistoryId is not int historyId) return;

            var history = await historyRepository.FindByIdAsync(historyId, cancellationToken)
                ?? throw new DomainValidationException($"History with ID {historyId} does not exist.");
            if (history.WorldId != location.WorldId)
            {
                throw new DomainValidationException($"History with ID {historyId} does not belong to this world.");
            }
        }

        /// <summary>Groups LocationType values by which C# class they materialize as under TPH —
        /// the 5 hierarchy subtypes each get their own group, the other 6 all share "Plain"
        /// (they're all just Location). Used to reject type changes that would require swapping
        /// the underlying CLR type, which isn't supported once a location has been created.</summary>
        public static string DiscriminatorGroup(LocationType type) => type switch
        {
            LocationType.Continent or LocationType.Region or LocationType.Country
                or LocationType.City or LocationType.District
                or LocationType.Lake or LocationType.Sea or LocationType.Ocean or LocationType.River
                or LocationType.Mountain or LocationType.MountainRange or LocationType.Swamp
                or LocationType.Desert or LocationType.Forest or LocationType.Cave or LocationType.Grassland
                => type.ToString(),
            _ => "Plain"
        };

        /// <summary>Source/Mouth (RiverEcosystem) moraju postojati u istom svijetu ako su postavljeni.</summary>
        public static async Task ValidateRiverEndpointsAsync(
            ILocationRepository repository, Location location, CancellationToken cancellationToken)
        {
            if (location is not RiverEcosystem river) return;

            if (river.SourceLocationId is int sourceId)
            {
                var source = await repository.FindByIdAsync(sourceId, cancellationToken);
                if (source == null || source.WorldId != river.WorldId)
                {
                    throw new DomainValidationException($"Source location with ID {sourceId} does not exist in this world.");
                }
            }

            if (river.MouthLocationId is int mouthId)
            {
                var mouth = await repository.FindByIdAsync(mouthId, cancellationToken);
                if (mouth == null || mouth.WorldId != river.WorldId)
                {
                    throw new DomainValidationException($"Mouth location with ID {mouthId} does not exist in this world.");
                }
            }
        }

        public static async Task ValidateSystemsAsync(
            IGovernmentSystemRepository governmentSystemRepository,
            ILegalSystemRepository legalSystemRepository,
            IEducationSystemRepository educationSystemRepository,
            Location location,
            CancellationToken cancellationToken)
        {
            var (governmentSystemId, legalSystemId, educationSystemId) = location switch
            {
                Country country => (country.GovernmentSystemId, country.LegalSystemId, country.EducationSystemId),
                City city => (city.GovernmentSystemId, city.LegalSystemId, city.EducationSystemId),
                _ => (null, null, null)
            };

            if (governmentSystemId is int gsId)
            {
                var governmentSystem = await governmentSystemRepository.FindByIdAsync(gsId, cancellationToken)
                    ?? throw new DomainValidationException($"Government system with ID {gsId} does not exist.");
                if (governmentSystem.WorldId != location.WorldId)
                {
                    throw new DomainValidationException($"Government system with ID {gsId} does not belong to this world.");
                }
            }

            if (legalSystemId is int lsId)
            {
                var legalSystem = await legalSystemRepository.FindByIdAsync(lsId, cancellationToken)
                    ?? throw new DomainValidationException($"Legal system with ID {lsId} does not exist.");
                if (legalSystem.WorldId != location.WorldId)
                {
                    throw new DomainValidationException($"Legal system with ID {lsId} does not belong to this world.");
                }
            }

            if (educationSystemId is int esId)
            {
                var educationSystem = await educationSystemRepository.FindByIdAsync(esId, cancellationToken)
                    ?? throw new DomainValidationException($"Education system with ID {esId} does not exist.");
                if (educationSystem.WorldId != location.WorldId)
                {
                    throw new DomainValidationException($"Education system with ID {esId} does not belong to this world.");
                }
            }
        }
    }
}
