using AutoMapper;
using ChronicleKeeper.Core.CQRS.Languages.Commands;
using ChronicleKeeper.Core.CQRS.Languages.Queries;
using ChronicleKeeper.Core.DTOs.Language;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Languages.Handlers
{
    public class GetAllLanguagesQueryHandler : IRequestHandler<GetAllLanguagesQuery, List<LanguageDto>>
    {
        private readonly ILanguageRepository _repository;
        private readonly IMapper _mapper;

        public GetAllLanguagesQueryHandler(ILanguageRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<LanguageDto>> Handle(GetAllLanguagesQuery request, CancellationToken cancellationToken)
        {
            var languages = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<LanguageDto>>(languages);
        }
    }

    public class GetLanguageByIdQueryHandler : IRequestHandler<GetLanguageByIdQuery, LanguageDetailsDto?>
    {
        private readonly ILanguageRepository _repository;
        private readonly IMapper _mapper;

        public GetLanguageByIdQueryHandler(ILanguageRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<LanguageDetailsDto?> Handle(GetLanguageByIdQuery request, CancellationToken cancellationToken)
        {
            var language = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return language == null ? null : _mapper.Map<LanguageDetailsDto>(language);
        }
    }

    public class CreateLanguageCommandHandler : IRequestHandler<CreateLanguageCommand, LanguageDto>
    {
        private readonly ILanguageRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateLanguageCommandHandler> _logger;

        public CreateLanguageCommandHandler(
            ILanguageRepository repository,
            IWorldRepository worldRepository,
            IMapper mapper,
            ILogger<CreateLanguageCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LanguageDto> Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
        {
            var dto = request.LanguageCreateDto;
            _logger.LogInformation("Creating new language: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Social.Cultures.Language>(dto), cancellationToken);
            return _mapper.Map<LanguageDto>(created);
        }
    }

    public class UpdateLanguageCommandHandler : IRequestHandler<UpdateLanguageCommand, LanguageDto>
    {
        private readonly ILanguageRepository _repository;
        private readonly IMapper _mapper;

        public UpdateLanguageCommandHandler(ILanguageRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<LanguageDto> Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
        {
            var language = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Language", request.Id);

            _mapper.Map(request.LanguageUpdateDto, language);
            var updated = await _repository.UpdateAsync(language, cancellationToken);
            return _mapper.Map<LanguageDto>(updated);
        }
    }

    public class DeleteLanguageCommandHandler : IRequestHandler<DeleteLanguageCommand, bool>
    {
        private readonly ILanguageRepository _repository;
        private readonly ILogger<DeleteLanguageCommandHandler> _logger;

        public DeleteLanguageCommandHandler(ILanguageRepository repository, ILogger<DeleteLanguageCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteLanguageCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting language with ID {Id}", request.Id);

            var inUse = await _repository.CountCulturesUsingLanguageAsync(request.Id, cancellationToken);
            if (inUse > 0)
            {
                throw new DomainValidationException(
                    $"This language is used by {inUse} culture(s). Reassign them first.");
            }

            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    public class AddLanguageNationCommandHandler : IRequestHandler<AddLanguageNationCommand, bool>
    {
        private readonly ILanguageRepository _repository;
        private readonly INationRepository _nationRepository;

        public AddLanguageNationCommandHandler(ILanguageRepository repository, INationRepository nationRepository)
        {
            _repository = repository;
            _nationRepository = nationRepository;
        }

        public async Task<bool> Handle(AddLanguageNationCommand request, CancellationToken cancellationToken)
        {
            var language = await _repository.FindByIdAsync(request.LanguageId, cancellationToken)
                ?? throw new EntityNotFoundException("Language", request.LanguageId);

            var nation = await _nationRepository.FindByIdAsync(request.NationId, cancellationToken)
                ?? throw new DomainValidationException($"Nation with ID {request.NationId} does not exist.");
            if (nation.WorldId != language.WorldId)
            {
                throw new DomainValidationException($"Nation with ID {request.NationId} does not belong to this world.");
            }

            if (await _repository.IsNationLinkedAsync(request.LanguageId, request.NationId, cancellationToken))
            {
                throw new DomainValidationException("This nation is already linked to the language.");
            }

            await _repository.AddNationAsync(request.LanguageId, request.NationId, cancellationToken);
            return true;
        }
    }

    public class RemoveLanguageNationCommandHandler : IRequestHandler<RemoveLanguageNationCommand, bool>
    {
        private readonly ILanguageRepository _repository;

        public RemoveLanguageNationCommandHandler(ILanguageRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(RemoveLanguageNationCommand request, CancellationToken cancellationToken)
        {
            return _repository.RemoveNationAsync(request.LanguageId, request.NationId, cancellationToken);
        }
    }
}
