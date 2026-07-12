using AutoMapper;
using ChronicleKeeper.Core.CQRS.Creatures.Commands;
using ChronicleKeeper.Core.CQRS.Creatures.Queries;
using ChronicleKeeper.Core.DTOs.Creature;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.Geography.Creatures;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Animals;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Fungi;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Plants;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Creatures.Handlers
{
    public class GetAllCreaturesQueryHandler : IRequestHandler<GetAllCreaturesQuery, List<CreatureDto>>
    {
        private readonly ICreatureRepository _repository;
        private readonly IMapper _mapper;

        public GetAllCreaturesQueryHandler(ICreatureRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<CreatureDto>> Handle(GetAllCreaturesQuery request, CancellationToken cancellationToken)
        {
            var creatures = await _repository.GetAllAsync(request.WorldId, request.Subtype, cancellationToken);
            return _mapper.Map<List<CreatureDto>>(creatures);
        }
    }

    public class GetCreatureByIdQueryHandler : IRequestHandler<GetCreatureByIdQuery, CreatureDetailsDto?>
    {
        private readonly ICreatureRepository _repository;
        private readonly IMapper _mapper;

        public GetCreatureByIdQueryHandler(ICreatureRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CreatureDetailsDto?> Handle(GetCreatureByIdQuery request, CancellationToken cancellationToken)
        {
            var creature = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return creature == null ? null : _mapper.Map<CreatureDetailsDto>(creature);
        }
    }

    public class CreateCreatureCommandHandler : IRequestHandler<CreateCreatureCommand, CreatureDto>
    {
        private static readonly string[] KnownSubtypes = { "Animal", "Plant", "Tree", "Crop", "Fungus" };

        private readonly ICreatureRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCreatureCommandHandler> _logger;

        public CreateCreatureCommandHandler(
            ICreatureRepository repository,
            IWorldRepository worldRepository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<CreateCreatureCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CreatureDto> Handle(CreateCreatureCommand request, CancellationToken cancellationToken)
        {
            var dto = request.CreatureCreateDto;
            _logger.LogInformation("Creating new creature: {Name} ({Subtype})", dto.Name, dto.Subtype);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            if (!KnownSubtypes.Contains(dto.Subtype))
            {
                throw new DomainValidationException($"Unknown creature subtype '{dto.Subtype}'. Expected one of: {string.Join(", ", KnownSubtypes)}.");
            }

            Creature creature = dto.Subtype switch
            {
                "Animal" => new Animal
                {
                    Diet = dto.Diet ?? default,
                    IsDomesticated = dto.IsDomesticated ?? false,
                    NumberOfLegs = dto.NumberOfLegs ?? 0,
                    HasWings = dto.HasWings ?? false,
                    HasMultipleHeads = dto.HasMultipleHeads ?? false,
                    HasRegeneration = dto.HasRegeneration ?? false,
                    IsSacred = dto.IsSacred ?? false,
                    IsMythical = dto.IsMythical ?? false,
                    IsEndangered = dto.IsEndangered ?? false,
                    Intelligence = dto.Intelligence ?? string.Empty,
                    SpecialAbilities = dto.SpecialAbilities ?? string.Empty,
                    IsPackAnimal = dto.IsPackAnimal ?? false,
                    IsAggressive = dto.IsAggressive ?? false,
                    IsSymbiotic = dto.IsSymbiotic ?? false
                },
                "Plant" => new Plant
                {
                    PlantType = dto.PlantType ?? default,
                    ScientificName = dto.ScientificName ?? string.Empty,
                    IsMedicinal = dto.IsMedicinal ?? false,
                    IsPoisonous = dto.IsPoisonous ?? false,
                    Sunlight = dto.Sunlight ?? default,
                    PreferredSoil = dto.PreferredSoil ?? default,
                    TemperatureRange = dto.TemperatureRange ?? default,
                    Rarity = dto.Rarity ?? default,
                    IsBioluminescent = dto.IsBioluminescent ?? false,
                    IsCarnivorous = dto.IsCarnivorous ?? false,
                    HasRegenerativeProperties = dto.HasRegenerativeProperties ?? false,
                    CanMove = dto.CanMove ?? false,
                    SpecialProperties = dto.SpecialProperties ?? string.Empty,
                    MythologicalSignificance = dto.MythologicalSignificance ?? string.Empty,
                    IsSymbiotic = dto.IsSymbiotic ?? false,
                    IsParasitic = dto.IsParasitic ?? false
                },
                "Tree" => new Tree
                {
                    PlantType = dto.PlantType ?? default,
                    ScientificName = dto.ScientificName ?? string.Empty,
                    IsMedicinal = dto.IsMedicinal ?? false,
                    IsPoisonous = dto.IsPoisonous ?? false,
                    Sunlight = dto.Sunlight ?? default,
                    PreferredSoil = dto.PreferredSoil ?? default,
                    TemperatureRange = dto.TemperatureRange ?? default,
                    Rarity = dto.Rarity ?? default,
                    IsBioluminescent = dto.IsBioluminescent ?? false,
                    IsCarnivorous = dto.IsCarnivorous ?? false,
                    HasRegenerativeProperties = dto.HasRegenerativeProperties ?? false,
                    CanMove = dto.CanMove ?? false,
                    SpecialProperties = dto.SpecialProperties ?? string.Empty,
                    MythologicalSignificance = dto.MythologicalSignificance ?? string.Empty,
                    IsSymbiotic = dto.IsSymbiotic ?? false,
                    IsParasitic = dto.IsParasitic ?? false,
                    MaxHeight = dto.MaxHeight ?? 0,
                    Lifespan = dto.Lifespan ?? 0,
                    LeafType = dto.LeafType ?? default
                },
                "Crop" => new Crop
                {
                    PlantType = dto.PlantType ?? default,
                    ScientificName = dto.ScientificName ?? string.Empty,
                    IsMedicinal = dto.IsMedicinal ?? false,
                    IsPoisonous = dto.IsPoisonous ?? false,
                    Sunlight = dto.Sunlight ?? default,
                    PreferredSoil = dto.PreferredSoil ?? default,
                    TemperatureRange = dto.TemperatureRange ?? default,
                    Rarity = dto.Rarity ?? default,
                    IsBioluminescent = dto.IsBioluminescent ?? false,
                    IsCarnivorous = dto.IsCarnivorous ?? false,
                    HasRegenerativeProperties = dto.HasRegenerativeProperties ?? false,
                    CanMove = dto.CanMove ?? false,
                    SpecialProperties = dto.SpecialProperties ?? string.Empty,
                    MythologicalSignificance = dto.MythologicalSignificance ?? string.Empty,
                    IsSymbiotic = dto.IsSymbiotic ?? false,
                    IsParasitic = dto.IsParasitic ?? false,
                    YieldPerHectare = dto.YieldPerHectare ?? 0,
                    CropType = dto.CropType ?? default,
                    IsDomesticated = dto.IsDomesticated ?? false
                },
                "Fungus" => new Fungus
                {
                    ScientificName = dto.ScientificName ?? string.Empty,
                    IsMedicinal = dto.IsMedicinal ?? false,
                    IsPoisonous = dto.IsPoisonous ?? false,
                    IsEdible = dto.IsEdible ?? false,
                    IsHallucinogenic = dto.IsHallucinogenic ?? false,
                    IsBioluminescent = dto.IsBioluminescent ?? false,
                    HasMutagenicProperties = dto.HasMutagenicProperties ?? false,
                    IsSymbiotic = dto.IsSymbiotic ?? false,
                    CanCommunicate = dto.CanCommunicate ?? false,
                    SpecialProperties = dto.SpecialProperties ?? string.Empty,
                    MythologicalSignificance = dto.MythologicalSignificance ?? string.Empty
                },
                _ => throw new DomainValidationException($"Unknown creature subtype '{dto.Subtype}'.")
            };

            creature.Name = dto.Name;
            creature.Description = dto.Description;
            creature.WorldId = dto.WorldId;
            creature.Type = dto.Type;
            creature.AverageLifespan = dto.AverageLifespan;
            creature.Height = dto.Height;
            creature.Weight = dto.Weight;
            creature.IsSentient = dto.IsSentient;
            creature.IsArtificial = dto.IsArtificial;
            creature.ArtificialOrigin = dto.ArtificialOrigin;
            creature.ParentCreatureId = dto.ParentCreatureId;
            creature.HistoryId = dto.HistoryId;

            await CreatureValidation.ValidateParentAsync(_repository, creature, cancellationToken);
            await CreatureValidation.ValidateHistoryAsync(_historyRepository, creature, cancellationToken);

            var created = await _repository.CreateAsync(creature, cancellationToken);
            _logger.LogInformation("Created creature with ID {Id}", created.Id);

            return _mapper.Map<CreatureDto>(created);
        }
    }

    public class UpdateCreatureCommandHandler : IRequestHandler<UpdateCreatureCommand, CreatureDto>
    {
        private readonly ICreatureRepository _repository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCreatureCommandHandler> _logger;

        public UpdateCreatureCommandHandler(
            ICreatureRepository repository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<UpdateCreatureCommandHandler> logger)
        {
            _repository = repository;
            _historyRepository = historyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CreatureDto> Handle(UpdateCreatureCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating creature with ID {Id}", request.Id);

            var creature = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Creature", request.Id);

            var dto = request.CreatureUpdateDto;

            creature.Name = dto.Name;
            creature.Description = dto.Description;
            creature.Type = dto.Type;
            creature.AverageLifespan = dto.AverageLifespan;
            creature.Height = dto.Height;
            creature.Weight = dto.Weight;
            creature.IsSentient = dto.IsSentient;
            creature.IsArtificial = dto.IsArtificial;
            creature.ArtificialOrigin = dto.ArtificialOrigin;
            creature.ParentCreatureId = dto.ParentCreatureId;
            creature.HistoryId = dto.HistoryId;

            switch (creature)
            {
                // Tree/Crop cases must come before Plant — both are Plant subclasses and would
                // otherwise match the Plant case first under C#'s top-to-bottom pattern matching.
                case Tree tree:
                    tree.PlantType = dto.PlantType ?? default;
                    tree.ScientificName = dto.ScientificName ?? string.Empty;
                    tree.IsMedicinal = dto.IsMedicinal ?? false;
                    tree.IsPoisonous = dto.IsPoisonous ?? false;
                    tree.Sunlight = dto.Sunlight ?? default;
                    tree.PreferredSoil = dto.PreferredSoil ?? default;
                    tree.TemperatureRange = dto.TemperatureRange ?? default;
                    tree.Rarity = dto.Rarity ?? default;
                    tree.IsBioluminescent = dto.IsBioluminescent ?? false;
                    tree.IsCarnivorous = dto.IsCarnivorous ?? false;
                    tree.HasRegenerativeProperties = dto.HasRegenerativeProperties ?? false;
                    tree.CanMove = dto.CanMove ?? false;
                    tree.SpecialProperties = dto.SpecialProperties ?? string.Empty;
                    tree.MythologicalSignificance = dto.MythologicalSignificance ?? string.Empty;
                    tree.IsSymbiotic = dto.IsSymbiotic ?? false;
                    tree.IsParasitic = dto.IsParasitic ?? false;
                    tree.MaxHeight = dto.MaxHeight ?? 0;
                    tree.Lifespan = dto.Lifespan ?? 0;
                    tree.LeafType = dto.LeafType ?? default;
                    break;

                case Crop crop:
                    crop.PlantType = dto.PlantType ?? default;
                    crop.ScientificName = dto.ScientificName ?? string.Empty;
                    crop.IsMedicinal = dto.IsMedicinal ?? false;
                    crop.IsPoisonous = dto.IsPoisonous ?? false;
                    crop.Sunlight = dto.Sunlight ?? default;
                    crop.PreferredSoil = dto.PreferredSoil ?? default;
                    crop.TemperatureRange = dto.TemperatureRange ?? default;
                    crop.Rarity = dto.Rarity ?? default;
                    crop.IsBioluminescent = dto.IsBioluminescent ?? false;
                    crop.IsCarnivorous = dto.IsCarnivorous ?? false;
                    crop.HasRegenerativeProperties = dto.HasRegenerativeProperties ?? false;
                    crop.CanMove = dto.CanMove ?? false;
                    crop.SpecialProperties = dto.SpecialProperties ?? string.Empty;
                    crop.MythologicalSignificance = dto.MythologicalSignificance ?? string.Empty;
                    crop.IsSymbiotic = dto.IsSymbiotic ?? false;
                    crop.IsParasitic = dto.IsParasitic ?? false;
                    crop.YieldPerHectare = dto.YieldPerHectare ?? 0;
                    crop.CropType = dto.CropType ?? default;
                    crop.IsDomesticated = dto.IsDomesticated ?? false;
                    break;

                case Plant plant:
                    plant.PlantType = dto.PlantType ?? default;
                    plant.ScientificName = dto.ScientificName ?? string.Empty;
                    plant.IsMedicinal = dto.IsMedicinal ?? false;
                    plant.IsPoisonous = dto.IsPoisonous ?? false;
                    plant.Sunlight = dto.Sunlight ?? default;
                    plant.PreferredSoil = dto.PreferredSoil ?? default;
                    plant.TemperatureRange = dto.TemperatureRange ?? default;
                    plant.Rarity = dto.Rarity ?? default;
                    plant.IsBioluminescent = dto.IsBioluminescent ?? false;
                    plant.IsCarnivorous = dto.IsCarnivorous ?? false;
                    plant.HasRegenerativeProperties = dto.HasRegenerativeProperties ?? false;
                    plant.CanMove = dto.CanMove ?? false;
                    plant.SpecialProperties = dto.SpecialProperties ?? string.Empty;
                    plant.MythologicalSignificance = dto.MythologicalSignificance ?? string.Empty;
                    plant.IsSymbiotic = dto.IsSymbiotic ?? false;
                    plant.IsParasitic = dto.IsParasitic ?? false;
                    break;

                case Animal animal:
                    animal.Diet = dto.Diet ?? default;
                    animal.IsDomesticated = dto.IsDomesticated ?? false;
                    animal.NumberOfLegs = dto.NumberOfLegs ?? 0;
                    animal.HasWings = dto.HasWings ?? false;
                    animal.HasMultipleHeads = dto.HasMultipleHeads ?? false;
                    animal.HasRegeneration = dto.HasRegeneration ?? false;
                    animal.IsSacred = dto.IsSacred ?? false;
                    animal.IsMythical = dto.IsMythical ?? false;
                    animal.IsEndangered = dto.IsEndangered ?? false;
                    animal.Intelligence = dto.Intelligence ?? string.Empty;
                    animal.SpecialAbilities = dto.SpecialAbilities ?? string.Empty;
                    animal.IsPackAnimal = dto.IsPackAnimal ?? false;
                    animal.IsAggressive = dto.IsAggressive ?? false;
                    animal.IsSymbiotic = dto.IsSymbiotic ?? false;
                    break;

                case Fungus fungus:
                    fungus.ScientificName = dto.ScientificName ?? string.Empty;
                    fungus.IsMedicinal = dto.IsMedicinal ?? false;
                    fungus.IsPoisonous = dto.IsPoisonous ?? false;
                    fungus.IsEdible = dto.IsEdible ?? false;
                    fungus.IsHallucinogenic = dto.IsHallucinogenic ?? false;
                    fungus.IsBioluminescent = dto.IsBioluminescent ?? false;
                    fungus.HasMutagenicProperties = dto.HasMutagenicProperties ?? false;
                    fungus.IsSymbiotic = dto.IsSymbiotic ?? false;
                    fungus.CanCommunicate = dto.CanCommunicate ?? false;
                    fungus.SpecialProperties = dto.SpecialProperties ?? string.Empty;
                    fungus.MythologicalSignificance = dto.MythologicalSignificance ?? string.Empty;
                    break;
            }

            await CreatureValidation.ValidateParentAsync(_repository, creature, cancellationToken);
            await CreatureValidation.ValidateHistoryAsync(_historyRepository, creature, cancellationToken);

            var updated = await _repository.UpdateAsync(creature, cancellationToken);
            return _mapper.Map<CreatureDto>(updated);
        }
    }

    public class DeleteCreatureCommandHandler : IRequestHandler<DeleteCreatureCommand, bool>
    {
        private readonly ICreatureRepository _repository;
        private readonly ILogger<DeleteCreatureCommandHandler> _logger;

        public DeleteCreatureCommandHandler(ICreatureRepository repository, ILogger<DeleteCreatureCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteCreatureCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting creature with ID {Id}", request.Id);

            if (await _repository.HasSubspeciesAsync(request.Id, cancellationToken))
            {
                throw new DomainValidationException(
                    "This creature has subspecies. Reparent or delete them first.");
            }

            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    public class AddCreatureCityCommandHandler : IRequestHandler<AddCreatureCityCommand, bool>
    {
        private readonly ICreatureRepository _repository;
        private readonly ILocationRepository _locationRepository;

        public AddCreatureCityCommandHandler(ICreatureRepository repository, ILocationRepository locationRepository)
        {
            _repository = repository;
            _locationRepository = locationRepository;
        }

        public async Task<bool> Handle(AddCreatureCityCommand request, CancellationToken cancellationToken)
        {
            var creature = await _repository.FindByIdAsync(request.CreatureId, cancellationToken)
                ?? throw new EntityNotFoundException("Creature", request.CreatureId);

            var city = await _locationRepository.FindByIdAsync(request.CityId, cancellationToken);
            if (city is not City || city.WorldId != creature.WorldId)
            {
                throw new DomainValidationException($"City with ID {request.CityId} does not exist in this world.");
            }

            if (await _repository.IsCityLinkedAsync(request.CreatureId, request.CityId, cancellationToken))
            {
                throw new DomainValidationException("This city is already linked to the creature.");
            }

            await _repository.AddCityAsync(request.CreatureId, request.CityId, cancellationToken);
            return true;
        }
    }

    public class RemoveCreatureCityCommandHandler : IRequestHandler<RemoveCreatureCityCommand, bool>
    {
        private readonly ICreatureRepository _repository;

        public RemoveCreatureCityCommandHandler(ICreatureRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(RemoveCreatureCityCommand request, CancellationToken cancellationToken)
        {
            return _repository.RemoveCityAsync(request.CreatureId, request.CityId, cancellationToken);
        }
    }

    internal static class CreatureValidation
    {
        /// <summary>Roditelj mora postojati u istom svijetu, ne smije biti samo stvorenje, i ne smije stvarati ciklus.</summary>
        public static async Task ValidateParentAsync(
            ICreatureRepository repository, Creature creature, CancellationToken cancellationToken)
        {
            if (creature.ParentCreatureId is not int parentId) return;

            if (parentId == creature.Id)
            {
                throw new DomainValidationException("A creature cannot be its own parent.");
            }
            var parent = await repository.FindByIdAsync(parentId, cancellationToken);
            if (parent == null || parent.WorldId != creature.WorldId)
            {
                throw new DomainValidationException($"Parent creature with ID {parentId} does not exist in this world.");
            }
            // Ciklus je moguć samo za postojeće stvorenje (novokreirano još nema podvrsta)
            if (creature.Id != 0 && await repository.WouldCreateCycleAsync(creature.Id, parentId, cancellationToken))
            {
                throw new DomainValidationException("This parent assignment would create a cycle in the creature hierarchy.");
            }
        }

        public static async Task ValidateHistoryAsync(
            IHistoryRepository historyRepository, Creature creature, CancellationToken cancellationToken)
        {
            if (creature.HistoryId is not int historyId) return;

            var history = await historyRepository.FindByIdAsync(historyId, cancellationToken)
                ?? throw new DomainValidationException($"History with ID {historyId} does not exist.");
            if (history.WorldId != creature.WorldId)
            {
                throw new DomainValidationException($"History with ID {historyId} does not belong to this world.");
            }
        }
    }
}
