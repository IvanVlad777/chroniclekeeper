using AutoMapper;
using ChronicleKeeper.Core.CQRS.ContentReferenceLinks.Commands;
using ChronicleKeeper.Core.CQRS.ContentReferenceLinks.Queries;
using ChronicleKeeper.Core.DTOs.ContentReferenceLink;
using ChronicleKeeper.Core.Entities.Content;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.ContentReferenceLinks.Handlers
{
    public class GetAllContentReferenceLinksQueryHandler : IRequestHandler<GetAllContentReferenceLinksQuery, List<ContentReferenceLinkDto>>
    {
        private readonly IReferenceRepository _repository;
        private readonly IMapper _mapper;

        public GetAllContentReferenceLinksQueryHandler(IReferenceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ContentReferenceLinkDto>> Handle(GetAllContentReferenceLinksQuery request, CancellationToken cancellationToken)
        {
            var references = await _repository.GetAllAsync(
                request.ContentId, request.ChapterId, request.EpisodeId,
                request.CharacterId, request.LocationId, request.FactionId, request.NationId,
                cancellationToken);
            return _mapper.Map<List<ContentReferenceLinkDto>>(references);
        }
    }

    public class GetContentReferenceLinkByIdQueryHandler : IRequestHandler<GetContentReferenceLinkByIdQuery, ContentReferenceLinkDto?>
    {
        private readonly IReferenceRepository _repository;
        private readonly IMapper _mapper;

        public GetContentReferenceLinkByIdQueryHandler(IReferenceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ContentReferenceLinkDto?> Handle(GetContentReferenceLinkByIdQuery request, CancellationToken cancellationToken)
        {
            var reference = await _repository.FindByIdAsync(request.Id, cancellationToken);
            return reference == null ? null : _mapper.Map<ContentReferenceLinkDto>(reference);
        }
    }

    public class CreateContentReferenceLinkCommandHandler : IRequestHandler<CreateContentReferenceLinkCommand, ContentReferenceLinkDto>
    {
        private readonly IReferenceRepository _repository;
        private readonly IContentRepository _contentRepository;
        private readonly IChapterRepository _chapterRepository;
        private readonly IEpisodeRepository _episodeRepository;
        private readonly ICharacterRepository _characterRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IFactionRepository _factionRepository;
        private readonly INationRepository _nationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateContentReferenceLinkCommandHandler> _logger;

        public CreateContentReferenceLinkCommandHandler(
            IReferenceRepository repository,
            IContentRepository contentRepository,
            IChapterRepository chapterRepository,
            IEpisodeRepository episodeRepository,
            ICharacterRepository characterRepository,
            ILocationRepository locationRepository,
            IFactionRepository factionRepository,
            INationRepository nationRepository,
            IMapper mapper,
            ILogger<CreateContentReferenceLinkCommandHandler> logger)
        {
            _repository = repository;
            _contentRepository = contentRepository;
            _chapterRepository = chapterRepository;
            _episodeRepository = episodeRepository;
            _characterRepository = characterRepository;
            _locationRepository = locationRepository;
            _factionRepository = factionRepository;
            _nationRepository = nationRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ContentReferenceLinkDto> Handle(CreateContentReferenceLinkCommand request, CancellationToken cancellationToken)
        {
            var dto = request.ContentReferenceLinkCreateDto;
            _logger.LogInformation("Creating new content reference link");

            var reference = _mapper.Map<Reference>(dto);
            await ContentReferenceLinkValidation.ValidateAsync(
                _contentRepository, _chapterRepository, _episodeRepository,
                _characterRepository, _locationRepository, _factionRepository, _nationRepository,
                reference, cancellationToken);

            var created = await _repository.CreateAsync(reference, cancellationToken);
            return _mapper.Map<ContentReferenceLinkDto>(created);
        }
    }

    public class UpdateContentReferenceLinkCommandHandler : IRequestHandler<UpdateContentReferenceLinkCommand, ContentReferenceLinkDto>
    {
        private readonly IReferenceRepository _repository;
        private readonly IContentRepository _contentRepository;
        private readonly IChapterRepository _chapterRepository;
        private readonly IEpisodeRepository _episodeRepository;
        private readonly ICharacterRepository _characterRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IFactionRepository _factionRepository;
        private readonly INationRepository _nationRepository;
        private readonly IMapper _mapper;

        public UpdateContentReferenceLinkCommandHandler(
            IReferenceRepository repository,
            IContentRepository contentRepository,
            IChapterRepository chapterRepository,
            IEpisodeRepository episodeRepository,
            ICharacterRepository characterRepository,
            ILocationRepository locationRepository,
            IFactionRepository factionRepository,
            INationRepository nationRepository,
            IMapper mapper)
        {
            _repository = repository;
            _contentRepository = contentRepository;
            _chapterRepository = chapterRepository;
            _episodeRepository = episodeRepository;
            _characterRepository = characterRepository;
            _locationRepository = locationRepository;
            _factionRepository = factionRepository;
            _nationRepository = nationRepository;
            _mapper = mapper;
        }

        public async Task<ContentReferenceLinkDto> Handle(UpdateContentReferenceLinkCommand request, CancellationToken cancellationToken)
        {
            var reference = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Reference", request.Id);

            _mapper.Map(request.ContentReferenceLinkUpdateDto, reference);
            await ContentReferenceLinkValidation.ValidateAsync(
                _contentRepository, _chapterRepository, _episodeRepository,
                _characterRepository, _locationRepository, _factionRepository, _nationRepository,
                reference, cancellationToken);

            var updated = await _repository.UpdateAsync(reference, cancellationToken);
            return _mapper.Map<ContentReferenceLinkDto>(updated);
        }
    }

    public class DeleteContentReferenceLinkCommandHandler : IRequestHandler<DeleteContentReferenceLinkCommand, bool>
    {
        private readonly IReferenceRepository _repository;
        private readonly ILogger<DeleteContentReferenceLinkCommandHandler> _logger;

        public DeleteContentReferenceLinkCommandHandler(IReferenceRepository repository, ILogger<DeleteContentReferenceLinkCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteContentReferenceLinkCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting content reference link with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    /// <summary>
    /// Each of the 7 FKs is validated independently, only if provided — mirrors
    /// EducationRecordHandlers' SchoolId/UniversityId looseness (no "exactly one per side"
    /// enforcement). Existence-only checks: Reference has no WorldId of its own to cross-check against.
    /// </summary>
    internal static class ContentReferenceLinkValidation
    {
        public static async Task ValidateAsync(
            IContentRepository contentRepository,
            IChapterRepository chapterRepository,
            IEpisodeRepository episodeRepository,
            ICharacterRepository characterRepository,
            ILocationRepository locationRepository,
            IFactionRepository factionRepository,
            INationRepository nationRepository,
            Reference reference,
            CancellationToken cancellationToken)
        {
            if (reference.ContentId is int contentId
                && await contentRepository.FindByIdAsync(contentId, cancellationToken) is null)
            {
                throw new DomainValidationException($"Content with ID {contentId} does not exist.");
            }

            if (reference.ChapterId is int chapterId
                && await chapterRepository.FindByIdAsync(chapterId, cancellationToken) is null)
            {
                throw new DomainValidationException($"Chapter with ID {chapterId} does not exist.");
            }

            if (reference.EpisodeId is int episodeId
                && await episodeRepository.FindByIdAsync(episodeId, cancellationToken) is null)
            {
                throw new DomainValidationException($"Episode with ID {episodeId} does not exist.");
            }

            if (reference.CharacterId is int characterId
                && await characterRepository.FindByIdAsync(characterId, cancellationToken) is null)
            {
                throw new DomainValidationException($"Character with ID {characterId} does not exist.");
            }

            if (reference.LocationId is int locationId
                && await locationRepository.FindByIdAsync(locationId, cancellationToken) is null)
            {
                throw new DomainValidationException($"Location with ID {locationId} does not exist.");
            }

            if (reference.FactionId is int factionId
                && await factionRepository.FindByIdAsync(factionId, cancellationToken) is null)
            {
                throw new DomainValidationException($"Faction with ID {factionId} does not exist.");
            }

            if (reference.NationId is int nationId
                && await nationRepository.FindByIdAsync(nationId, cancellationToken) is null)
            {
                throw new DomainValidationException($"Nation with ID {nationId} does not exist.");
            }
        }
    }
}
