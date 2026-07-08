using AutoMapper;
using ChronicleKeeper.Core.CQRS.Worlds.Commands;
using ChronicleKeeper.Core.CQRS.Worlds.Queries;
using ChronicleKeeper.Core.DTOs.World;
using ChronicleKeeper.Core.Entities.Worlds;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Worlds.Handlers
{
    internal static class WorldOwnership
    {
        /// <summary>Pisanje po svijetu smije samo vlasnik ili site admin (Admin/SuperAdmin).</summary>
        public static void EnsureCanModify(World world, string requesterId, bool requesterIsAdmin)
        {
            if (!requesterIsAdmin && world.OwnerId != requesterId)
            {
                throw new ForbiddenAccessException("You do not own this world.");
            }
        }
    }

    public class GetAllWorldsQueryHandler : IRequestHandler<GetAllWorldsQuery, List<WorldDto>>
    {
        private readonly IWorldRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllWorldsQueryHandler> _logger;

        public GetAllWorldsQueryHandler(IWorldRepository repository, IMapper mapper, ILogger<GetAllWorldsQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<WorldDto>> Handle(GetAllWorldsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all worlds");

            var worlds = request.OwnerId == null
                ? await _repository.GetAllAsync(cancellationToken)
                : await _repository.GetByOwnerAsync(request.OwnerId, cancellationToken);

            return _mapper.Map<List<WorldDto>>(worlds);
        }
    }

    public class GetWorldByIdQueryHandler : IRequestHandler<GetWorldByIdQuery, WorldDto?>
    {
        private readonly IWorldRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetWorldByIdQueryHandler> _logger;

        public GetWorldByIdQueryHandler(IWorldRepository repository, IMapper mapper, ILogger<GetWorldByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<WorldDto?> Handle(GetWorldByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching world with ID {Id}", request.Id);

            var world = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return world == null ? null : _mapper.Map<WorldDto>(world);
        }
    }

    public class CreateWorldCommandHandler : IRequestHandler<CreateWorldCommand, WorldDto>
    {
        private readonly IWorldRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateWorldCommandHandler> _logger;

        public CreateWorldCommandHandler(IWorldRepository repository, IMapper mapper, ILogger<CreateWorldCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<WorldDto> Handle(CreateWorldCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating new world: {Name}", request.WorldCreateDto.Name);

            var world = _mapper.Map<World>(request.WorldCreateDto);
            world.OwnerId = request.OwnerId;

            var created = await _repository.CreateAsync(world, cancellationToken);
            _logger.LogInformation("Created world with ID {Id}", created.Id);

            return _mapper.Map<WorldDto>(created);
        }
    }

    public class UpdateWorldCommandHandler : IRequestHandler<UpdateWorldCommand, WorldDto>
    {
        private readonly IWorldRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateWorldCommandHandler> _logger;

        public UpdateWorldCommandHandler(IWorldRepository repository, IMapper mapper, ILogger<UpdateWorldCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<WorldDto> Handle(UpdateWorldCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating world with ID {Id}", request.Id);

            var world = await _repository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("World", request.Id);

            WorldOwnership.EnsureCanModify(world, request.RequesterId, request.RequesterIsAdmin);

            _mapper.Map(request.WorldUpdateDto, world);
            var updated = await _repository.UpdateAsync(world, cancellationToken);

            return _mapper.Map<WorldDto>(updated);
        }
    }

    public class DeleteWorldCommandHandler : IRequestHandler<DeleteWorldCommand, bool>
    {
        private readonly IWorldRepository _repository;
        private readonly ILogger<DeleteWorldCommandHandler> _logger;

        public DeleteWorldCommandHandler(IWorldRepository repository, ILogger<DeleteWorldCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteWorldCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting world with ID {Id} and ALL of its data", request.Id);

            var world = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (world == null)
            {
                _logger.LogWarning("World with ID {Id} not found for deletion", request.Id);
                return false;
            }

            WorldOwnership.EnsureCanModify(world, request.RequesterId, request.RequesterIsAdmin);

            await _repository.DeleteAsync(request.Id, cancellationToken);
            _logger.LogInformation("Deleted world with ID {Id}", request.Id);
            return true;
        }
    }
}
