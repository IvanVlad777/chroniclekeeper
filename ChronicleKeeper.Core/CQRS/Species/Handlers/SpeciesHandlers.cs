using AutoMapper;
using ChronicleKeeper.Core.CQRS.Species.Commands;
using ChronicleKeeper.Core.CQRS.Species.Queries;
using ChronicleKeeper.Core.DTOs.Species;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Species.Handlers
{
    // ----- Species -----

    public class GetAllSpeciesQueryHandler : IRequestHandler<GetAllSpeciesQuery, List<SpeciesDto>>
    {
        private readonly ISpeciesRepository _repository;
        private readonly IMapper _mapper;

        public GetAllSpeciesQueryHandler(ISpeciesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<SpeciesDto>> Handle(GetAllSpeciesQuery request, CancellationToken cancellationToken)
        {
            var species = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<SpeciesDto>>(species);
        }
    }

    public class GetSpeciesByIdQueryHandler : IRequestHandler<GetSpeciesByIdQuery, SpeciesDetailsDto?>
    {
        private readonly ISpeciesRepository _repository;
        private readonly IMapper _mapper;

        public GetSpeciesByIdQueryHandler(ISpeciesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SpeciesDetailsDto?> Handle(GetSpeciesByIdQuery request, CancellationToken cancellationToken)
        {
            var species = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return species == null ? null : _mapper.Map<SpeciesDetailsDto>(species);
        }
    }

    public class CreateSpeciesCommandHandler : IRequestHandler<CreateSpeciesCommand, SpeciesDto>
    {
        private readonly ISpeciesRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSpeciesCommandHandler> _logger;

        public CreateSpeciesCommandHandler(
            ISpeciesRepository repository,
            IWorldRepository worldRepository,
            IMapper mapper,
            ILogger<CreateSpeciesCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SpeciesDto> Handle(CreateSpeciesCommand request, CancellationToken cancellationToken)
        {
            var dto = request.SpeciesCreateDto;
            _logger.LogInformation("Creating new species: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            var created = await _repository.CreateAsync(_mapper.Map<SapientSpecies>(dto), cancellationToken);
            return _mapper.Map<SpeciesDto>(created);
        }
    }

    public class UpdateSpeciesCommandHandler : IRequestHandler<UpdateSpeciesCommand, SpeciesDto>
    {
        private readonly ISpeciesRepository _repository;
        private readonly IMapper _mapper;

        public UpdateSpeciesCommandHandler(ISpeciesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SpeciesDto> Handle(UpdateSpeciesCommand request, CancellationToken cancellationToken)
        {
            var species = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Species", request.Id);

            _mapper.Map(request.SpeciesUpdateDto, species);
            var updated = await _repository.UpdateAsync(species, cancellationToken);
            return _mapper.Map<SpeciesDto>(updated);
        }
    }

    public class DeleteSpeciesCommandHandler : IRequestHandler<DeleteSpeciesCommand, bool>
    {
        private readonly ISpeciesRepository _repository;
        private readonly ILogger<DeleteSpeciesCommandHandler> _logger;

        public DeleteSpeciesCommandHandler(ISpeciesRepository repository, ILogger<DeleteSpeciesCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteSpeciesCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting species with ID {Id}", request.Id);

            var inUse = await _repository.CountCharactersUsingSpeciesAsync(request.Id, cancellationToken);
            if (inUse > 0)
            {
                throw new DomainValidationException(
                    $"This species (or one of its races) is used by {inUse} character(s). Reassign them first.");
            }

            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    // ----- Races -----

    public class GetAllRacesQueryHandler : IRequestHandler<GetAllRacesQuery, List<RaceDto>>
    {
        private readonly ISpeciesRepository _repository;
        private readonly IMapper _mapper;

        public GetAllRacesQueryHandler(ISpeciesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<RaceDto>> Handle(GetAllRacesQuery request, CancellationToken cancellationToken)
        {
            var races = await _repository.GetRacesAsync(request.WorldId, request.SpeciesId, cancellationToken);
            return _mapper.Map<List<RaceDto>>(races);
        }
    }

    public class GetRaceByIdQueryHandler : IRequestHandler<GetRaceByIdQuery, RaceDto?>
    {
        private readonly ISpeciesRepository _repository;
        private readonly IMapper _mapper;

        public GetRaceByIdQueryHandler(ISpeciesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<RaceDto?> Handle(GetRaceByIdQuery request, CancellationToken cancellationToken)
        {
            var race = await _repository.FindRaceByIdAsync(request.Id, cancellationToken);
            return race == null ? null : _mapper.Map<RaceDto>(race);
        }
    }

    public class CreateRaceCommandHandler : IRequestHandler<CreateRaceCommand, RaceDto>
    {
        private readonly ISpeciesRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateRaceCommandHandler> _logger;

        public CreateRaceCommandHandler(ISpeciesRepository repository, IMapper mapper, ILogger<CreateRaceCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RaceDto> Handle(CreateRaceCommand request, CancellationToken cancellationToken)
        {
            var dto = request.RaceCreateDto;
            _logger.LogInformation("Creating new race: {Name}", dto.Name);

            var species = await _repository.FindByIdAsync(dto.SapientSpeciesId, cancellationToken)
                ?? throw new DomainValidationException($"Species with ID {dto.SapientSpeciesId} does not exist.");

            var race = _mapper.Map<Race>(dto);
            race.WorldId = species.WorldId; // svijet rase uvijek = svijet vrste

            var created = await _repository.CreateRaceAsync(race, cancellationToken);
            return _mapper.Map<RaceDto>(created);
        }
    }

    public class UpdateRaceCommandHandler : IRequestHandler<UpdateRaceCommand, RaceDto>
    {
        private readonly ISpeciesRepository _repository;
        private readonly IMapper _mapper;

        public UpdateRaceCommandHandler(ISpeciesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<RaceDto> Handle(UpdateRaceCommand request, CancellationToken cancellationToken)
        {
            var race = await _repository.FindRaceByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Race", request.Id);

            _mapper.Map(request.RaceUpdateDto, race);
            var updated = await _repository.UpdateRaceAsync(race, cancellationToken);
            return _mapper.Map<RaceDto>(updated);
        }
    }

    public class DeleteRaceCommandHandler : IRequestHandler<DeleteRaceCommand, bool>
    {
        private readonly ISpeciesRepository _repository;
        private readonly ILogger<DeleteRaceCommandHandler> _logger;

        public DeleteRaceCommandHandler(ISpeciesRepository repository, ILogger<DeleteRaceCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteRaceCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting race with ID {Id}", request.Id);

            var inUse = await _repository.CountCharactersUsingRaceAsync(request.Id, cancellationToken);
            if (inUse > 0)
            {
                throw new DomainValidationException(
                    $"This race is used by {inUse} character(s). Reassign them first.");
            }

            return await _repository.DeleteRaceAsync(request.Id, cancellationToken);
        }
    }
}
