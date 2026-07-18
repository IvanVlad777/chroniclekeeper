using AutoMapper;
using ChronicleKeeper.Core.CQRS.SocialHierarchies.Commands;
using ChronicleKeeper.Core.CQRS.SocialHierarchies.Queries;
using ChronicleKeeper.Core.DTOs.SocialHierarchy;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.SocialHierarchies.Handlers
{
    public class GetAllSocialHierarchiesQueryHandler : IRequestHandler<GetAllSocialHierarchiesQuery, List<SocialHierarchyDto>>
    {
        private readonly ISocialHierarchyRepository _repository;
        private readonly IMapper _mapper;

        public GetAllSocialHierarchiesQueryHandler(ISocialHierarchyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<SocialHierarchyDto>> Handle(GetAllSocialHierarchiesQuery request, CancellationToken cancellationToken)
        {
            var hierarchies = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<SocialHierarchyDto>>(hierarchies);
        }
    }

    public class GetSocialHierarchyByIdQueryHandler : IRequestHandler<GetSocialHierarchyByIdQuery, SocialHierarchyDetailsDto?>
    {
        private readonly ISocialHierarchyRepository _repository;
        private readonly IMapper _mapper;

        public GetSocialHierarchyByIdQueryHandler(ISocialHierarchyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SocialHierarchyDetailsDto?> Handle(GetSocialHierarchyByIdQuery request, CancellationToken cancellationToken)
        {
            var hierarchy = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return hierarchy == null ? null : _mapper.Map<SocialHierarchyDetailsDto>(hierarchy);
        }
    }

    public class CreateSocialHierarchyCommandHandler : IRequestHandler<CreateSocialHierarchyCommand, SocialHierarchyDto>
    {
        private readonly ISocialHierarchyRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSocialHierarchyCommandHandler> _logger;

        public CreateSocialHierarchyCommandHandler(
            ISocialHierarchyRepository repository,
            IWorldRepository worldRepository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<CreateSocialHierarchyCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SocialHierarchyDto> Handle(CreateSocialHierarchyCommand request, CancellationToken cancellationToken)
        {
            var dto = request.SocialHierarchyCreateDto;
            _logger.LogInformation("Creating new social hierarchy: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            var hierarchy = _mapper.Map<Entities.Social.Structure.SocialHierarchy>(dto);
            await TailValidation.ValidateHistoryAsync(_historyRepository, hierarchy.HistoryId, hierarchy.WorldId, cancellationToken);

            var created = await _repository.CreateAsync(hierarchy, cancellationToken);
            return _mapper.Map<SocialHierarchyDto>(created);
        }
    }

    public class UpdateSocialHierarchyCommandHandler : IRequestHandler<UpdateSocialHierarchyCommand, SocialHierarchyDto>
    {
        private readonly ISocialHierarchyRepository _repository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public UpdateSocialHierarchyCommandHandler(
            ISocialHierarchyRepository repository,
            IHistoryRepository historyRepository,
            IMapper mapper)
        {
            _repository = repository;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public async Task<SocialHierarchyDto> Handle(UpdateSocialHierarchyCommand request, CancellationToken cancellationToken)
        {
            var hierarchy = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("SocialHierarchy", request.Id);

            _mapper.Map(request.SocialHierarchyUpdateDto, hierarchy);
            await TailValidation.ValidateHistoryAsync(_historyRepository, hierarchy.HistoryId, hierarchy.WorldId, cancellationToken);

            var updated = await _repository.UpdateAsync(hierarchy, cancellationToken);
            return _mapper.Map<SocialHierarchyDto>(updated);
        }
    }

    public class DeleteSocialHierarchyCommandHandler : IRequestHandler<DeleteSocialHierarchyCommand, bool>
    {
        private readonly ISocialHierarchyRepository _repository;
        private readonly ILogger<DeleteSocialHierarchyCommandHandler> _logger;

        public DeleteSocialHierarchyCommandHandler(ISocialHierarchyRepository repository, ILogger<DeleteSocialHierarchyCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteSocialHierarchyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting social hierarchy with ID {Id}", request.Id);
            // No delete-guard: SocialClass.SocialHierarchyId / Nation.SocialHierarchyId are SetNull.
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    /// <summary>Shared same-world History validation for the R4 "tail" entities.</summary>
    internal static class TailValidation
    {
        public static async Task ValidateHistoryAsync(
            IHistoryRepository historyRepository, int? historyId, int worldId, CancellationToken cancellationToken)
        {
            if (historyId is not int hid) return;

            var history = await historyRepository.FindByIdAsync(hid, cancellationToken)
                ?? throw new DomainValidationException($"History with ID {hid} does not exist.");
            if (history.WorldId != worldId)
            {
                throw new DomainValidationException($"History with ID {hid} does not belong to this world.");
            }
        }
    }
}
