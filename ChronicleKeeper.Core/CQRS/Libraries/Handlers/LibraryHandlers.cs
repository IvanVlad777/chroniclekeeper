using AutoMapper;
using ChronicleKeeper.Core.CQRS.Libraries.Commands;
using ChronicleKeeper.Core.CQRS.Libraries.Queries;
using ChronicleKeeper.Core.DTOs.Library;
using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Libraries.Handlers
{
    public class GetAllLibrariesQueryHandler : IRequestHandler<GetAllLibrariesQuery, List<LibraryDto>>
    {
        private readonly ILibraryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllLibrariesQueryHandler(ILibraryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<LibraryDto>> Handle(GetAllLibrariesQuery request, CancellationToken cancellationToken)
        {
            var libraries = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<LibraryDto>>(libraries);
        }
    }

    public class GetLibraryByIdQueryHandler : IRequestHandler<GetLibraryByIdQuery, LibraryDetailsDto?>
    {
        private readonly ILibraryRepository _repository;
        private readonly IMapper _mapper;

        public GetLibraryByIdQueryHandler(ILibraryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<LibraryDetailsDto?> Handle(GetLibraryByIdQuery request, CancellationToken cancellationToken)
        {
            var library = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return library == null ? null : _mapper.Map<LibraryDetailsDto>(library);
        }
    }

    public class CreateLibraryCommandHandler : IRequestHandler<CreateLibraryCommand, LibraryDto>
    {
        private readonly ILibraryRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IUniversityRepository _universityRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateLibraryCommandHandler> _logger;

        public CreateLibraryCommandHandler(
            ILibraryRepository repository,
            IWorldRepository worldRepository,
            IUniversityRepository universityRepository,
            ILocationRepository locationRepository,
            IMapper mapper,
            ILogger<CreateLibraryCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _universityRepository = universityRepository;
            _locationRepository = locationRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LibraryDto> Handle(CreateLibraryCommand request, CancellationToken cancellationToken)
        {
            var dto = request.LibraryCreateDto;
            _logger.LogInformation("Creating new library: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            if (dto.UniversityId is int universityId
                && !await _universityRepository.ExistsInWorldAsync(universityId, dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"University with ID {universityId} does not exist in this world.");
            }

            if (dto.LocationId is int locationId
                && !await _locationRepository.ExistsInWorldAsync(locationId, dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"Location with ID {locationId} does not exist in this world.");
            }

            var library = _mapper.Map<Library>(dto);
            var created = await _repository.CreateAsync(library, cancellationToken);

            var withReferences = await _repository.GetByIdAsync(created.Id, cancellationToken) ?? created;
            return _mapper.Map<LibraryDto>(withReferences);
        }
    }

    public class UpdateLibraryCommandHandler : IRequestHandler<UpdateLibraryCommand, LibraryDto>
    {
        private readonly ILibraryRepository _repository;
        private readonly IUniversityRepository _universityRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public UpdateLibraryCommandHandler(
            ILibraryRepository repository,
            IUniversityRepository universityRepository,
            ILocationRepository locationRepository,
            IMapper mapper)
        {
            _repository = repository;
            _universityRepository = universityRepository;
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public async Task<LibraryDto> Handle(UpdateLibraryCommand request, CancellationToken cancellationToken)
        {
            var library = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Library", request.Id);

            var dto = request.LibraryUpdateDto;

            if (dto.UniversityId is int universityId
                && !await _universityRepository.ExistsInWorldAsync(universityId, library.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"University with ID {universityId} does not exist in this world.");
            }

            if (dto.LocationId is int locationId
                && !await _locationRepository.ExistsInWorldAsync(locationId, library.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"Location with ID {locationId} does not exist in this world.");
            }

            _mapper.Map(dto, library);
            var updated = await _repository.UpdateAsync(library, cancellationToken);

            var withReferences = await _repository.GetByIdAsync(updated.Id, cancellationToken) ?? updated;
            return _mapper.Map<LibraryDto>(withReferences);
        }
    }

    public class DeleteLibraryCommandHandler : IRequestHandler<DeleteLibraryCommand, bool>
    {
        private readonly ILibraryRepository _repository;
        private readonly ILogger<DeleteLibraryCommandHandler> _logger;

        public DeleteLibraryCommandHandler(ILibraryRepository repository, ILogger<DeleteLibraryCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteLibraryCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting library with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    public class AddLibraryScholarCommandHandler : IRequestHandler<AddLibraryScholarCommand, bool>
    {
        private readonly ILibraryRepository _repository;
        private readonly ICharacterRepository _characterRepository;

        public AddLibraryScholarCommandHandler(ILibraryRepository repository, ICharacterRepository characterRepository)
        {
            _repository = repository;
            _characterRepository = characterRepository;
        }

        public async Task<bool> Handle(AddLibraryScholarCommand request, CancellationToken cancellationToken)
        {
            var library = await _repository.FindByIdAsync(request.LibraryId, cancellationToken)
                ?? throw new EntityNotFoundException("Library", request.LibraryId);
            if (!await _characterRepository.ExistsInWorldAsync(request.CharacterId, library.WorldId, cancellationToken))
                throw new DomainValidationException($"Character with ID {request.CharacterId} does not exist in this world.");
            if (await _repository.IsScholarLinkedAsync(request.LibraryId, request.CharacterId, cancellationToken))
                throw new DomainValidationException("This character is already a scholar of the library.");

            await _repository.AddScholarAsync(request.LibraryId, request.CharacterId, cancellationToken);
            return true;
        }
    }

    public class RemoveLibraryScholarCommandHandler : IRequestHandler<RemoveLibraryScholarCommand, bool>
    {
        private readonly ILibraryRepository _repository;
        public RemoveLibraryScholarCommandHandler(ILibraryRepository repository) => _repository = repository;
        public Task<bool> Handle(RemoveLibraryScholarCommand request, CancellationToken cancellationToken)
            => _repository.RemoveScholarAsync(request.LibraryId, request.CharacterId, cancellationToken);
    }
}
