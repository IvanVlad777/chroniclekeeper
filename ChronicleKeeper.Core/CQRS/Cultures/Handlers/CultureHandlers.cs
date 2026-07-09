using AutoMapper;
using ChronicleKeeper.Core.CQRS.Cultures.Commands;
using ChronicleKeeper.Core.CQRS.Cultures.Queries;
using ChronicleKeeper.Core.DTOs.Culture;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Cultures.Handlers
{
    public class GetAllCulturesQueryHandler : IRequestHandler<GetAllCulturesQuery, List<CultureDto>>
    {
        private readonly ICultureRepository _repository;
        private readonly IMapper _mapper;

        public GetAllCulturesQueryHandler(ICultureRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<CultureDto>> Handle(GetAllCulturesQuery request, CancellationToken cancellationToken)
        {
            var cultures = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<CultureDto>>(cultures);
        }
    }

    public class GetCultureByIdQueryHandler : IRequestHandler<GetCultureByIdQuery, CultureDetailsDto?>
    {
        private readonly ICultureRepository _repository;
        private readonly IMapper _mapper;

        public GetCultureByIdQueryHandler(ICultureRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CultureDetailsDto?> Handle(GetCultureByIdQuery request, CancellationToken cancellationToken)
        {
            var culture = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return culture == null ? null : _mapper.Map<CultureDetailsDto>(culture);
        }
    }

    public class CreateCultureCommandHandler : IRequestHandler<CreateCultureCommand, CultureDto>
    {
        private readonly ICultureRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IReligionRepository _religionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCultureCommandHandler> _logger;

        public CreateCultureCommandHandler(
            ICultureRepository repository,
            IWorldRepository worldRepository,
            ILanguageRepository languageRepository,
            IReligionRepository religionRepository,
            IMapper mapper,
            ILogger<CreateCultureCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _languageRepository = languageRepository;
            _religionRepository = religionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CultureDto> Handle(CreateCultureCommand request, CancellationToken cancellationToken)
        {
            var dto = request.CultureCreateDto;
            _logger.LogInformation("Creating new culture: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            var language = await _languageRepository.FindByIdAsync(dto.LanguageId, cancellationToken)
                ?? throw new DomainValidationException($"Language with ID {dto.LanguageId} does not exist.");
            if (language.WorldId != dto.WorldId)
            {
                throw new DomainValidationException($"Language with ID {dto.LanguageId} does not belong to this world.");
            }

            if (dto.ReligionId is int religionId)
            {
                var religion = await _religionRepository.FindByIdAsync(religionId, cancellationToken)
                    ?? throw new DomainValidationException($"Religion with ID {religionId} does not exist.");
                if (religion.WorldId != dto.WorldId)
                {
                    throw new DomainValidationException($"Religion with ID {religionId} does not belong to this world.");
                }
            }

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Social.Cultures.Culture>(dto), cancellationToken);
            return _mapper.Map<CultureDto>(created);
        }
    }

    public class UpdateCultureCommandHandler : IRequestHandler<UpdateCultureCommand, CultureDto>
    {
        private readonly ICultureRepository _repository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IReligionRepository _religionRepository;
        private readonly IMapper _mapper;

        public UpdateCultureCommandHandler(
            ICultureRepository repository,
            ILanguageRepository languageRepository,
            IReligionRepository religionRepository,
            IMapper mapper)
        {
            _repository = repository;
            _languageRepository = languageRepository;
            _religionRepository = religionRepository;
            _mapper = mapper;
        }

        public async Task<CultureDto> Handle(UpdateCultureCommand request, CancellationToken cancellationToken)
        {
            var culture = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Culture", request.Id);

            var dto = request.CultureUpdateDto;

            var language = await _languageRepository.FindByIdAsync(dto.LanguageId, cancellationToken)
                ?? throw new DomainValidationException($"Language with ID {dto.LanguageId} does not exist.");
            if (language.WorldId != culture.WorldId)
            {
                throw new DomainValidationException($"Language with ID {dto.LanguageId} does not belong to this world.");
            }

            if (dto.ReligionId is int religionId)
            {
                var religion = await _religionRepository.FindByIdAsync(religionId, cancellationToken)
                    ?? throw new DomainValidationException($"Religion with ID {religionId} does not exist.");
                if (religion.WorldId != culture.WorldId)
                {
                    throw new DomainValidationException($"Religion with ID {religionId} does not belong to this world.");
                }
            }

            _mapper.Map(dto, culture);
            var updated = await _repository.UpdateAsync(culture, cancellationToken);
            return _mapper.Map<CultureDto>(updated);
        }
    }

    public class DeleteCultureCommandHandler : IRequestHandler<DeleteCultureCommand, bool>
    {
        private readonly ICultureRepository _repository;
        private readonly ILogger<DeleteCultureCommandHandler> _logger;

        public DeleteCultureCommandHandler(ICultureRepository repository, ILogger<DeleteCultureCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteCultureCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting culture with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
