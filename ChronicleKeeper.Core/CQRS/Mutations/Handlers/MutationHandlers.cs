using AutoMapper;
using ChronicleKeeper.Core.CQRS.Mutations.Commands;
using ChronicleKeeper.Core.CQRS.Mutations.Queries;
using ChronicleKeeper.Core.CQRS.SocialHierarchies.Handlers;
using ChronicleKeeper.Core.DTOs.Mutation;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Mutations.Handlers
{
    public class GetAllMutationsQueryHandler : IRequestHandler<GetAllMutationsQuery, List<MutationDto>>
    {
        private readonly IMutationRepository _repository;
        private readonly IMapper _mapper;

        public GetAllMutationsQueryHandler(IMutationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<MutationDto>> Handle(GetAllMutationsQuery request, CancellationToken cancellationToken)
        {
            var mutations = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<MutationDto>>(mutations);
        }
    }

    public class GetMutationByIdQueryHandler : IRequestHandler<GetMutationByIdQuery, MutationDetailsDto?>
    {
        private readonly IMutationRepository _repository;
        private readonly IMapper _mapper;

        public GetMutationByIdQueryHandler(IMutationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<MutationDetailsDto?> Handle(GetMutationByIdQuery request, CancellationToken cancellationToken)
        {
            var mutation = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return mutation == null ? null : _mapper.Map<MutationDetailsDto>(mutation);
        }
    }

    public class CreateMutationCommandHandler : IRequestHandler<CreateMutationCommand, MutationDto>
    {
        private readonly IMutationRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly ICreatureRepository _creatureRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateMutationCommandHandler> _logger;

        public CreateMutationCommandHandler(
            IMutationRepository repository,
            IWorldRepository worldRepository,
            ICreatureRepository creatureRepository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<CreateMutationCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _creatureRepository = creatureRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<MutationDto> Handle(CreateMutationCommand request, CancellationToken cancellationToken)
        {
            var dto = request.MutationCreateDto;
            _logger.LogInformation("Creating new mutation: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            var mutation = _mapper.Map<Entities.Miscellaneous.Mutation>(dto);
            await ValidateMutantCreatureAsync(_creatureRepository, mutation.MutantCreatureId, mutation.WorldId, cancellationToken);
            await TailValidation.ValidateHistoryAsync(_historyRepository, mutation.HistoryId, mutation.WorldId, cancellationToken);

            var created = await _repository.CreateAsync(mutation, cancellationToken);
            return _mapper.Map<MutationDto>(created);
        }

        internal static async Task ValidateMutantCreatureAsync(
            ICreatureRepository creatureRepository, int? creatureId, int worldId, CancellationToken cancellationToken)
        {
            if (creatureId is not int cid) return;

            var creature = await creatureRepository.FindByIdAsync(cid, cancellationToken);
            if (creature == null || creature.WorldId != worldId)
            {
                throw new DomainValidationException($"Creature with ID {cid} does not exist in this world.");
            }
        }
    }

    public class UpdateMutationCommandHandler : IRequestHandler<UpdateMutationCommand, MutationDto>
    {
        private readonly IMutationRepository _repository;
        private readonly ICreatureRepository _creatureRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public UpdateMutationCommandHandler(
            IMutationRepository repository,
            ICreatureRepository creatureRepository,
            IHistoryRepository historyRepository,
            IMapper mapper)
        {
            _repository = repository;
            _creatureRepository = creatureRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public async Task<MutationDto> Handle(UpdateMutationCommand request, CancellationToken cancellationToken)
        {
            var mutation = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Mutation", request.Id);

            _mapper.Map(request.MutationUpdateDto, mutation);
            await CreateMutationCommandHandler.ValidateMutantCreatureAsync(_creatureRepository, mutation.MutantCreatureId, mutation.WorldId, cancellationToken);
            await TailValidation.ValidateHistoryAsync(_historyRepository, mutation.HistoryId, mutation.WorldId, cancellationToken);

            var updated = await _repository.UpdateAsync(mutation, cancellationToken);
            return _mapper.Map<MutationDto>(updated);
        }
    }

    public class DeleteMutationCommandHandler : IRequestHandler<DeleteMutationCommand, bool>
    {
        private readonly IMutationRepository _repository;
        private readonly ILogger<DeleteMutationCommandHandler> _logger;

        public DeleteMutationCommandHandler(IMutationRepository repository, ILogger<DeleteMutationCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteMutationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting mutation with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
