using AutoMapper;
using ChronicleKeeper.Core.CQRS.SocialClasses.Commands;
using ChronicleKeeper.Core.CQRS.SocialClasses.Queries;
using ChronicleKeeper.Core.DTOs.SocialClass;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.SocialClasses.Handlers
{
    public class GetAllSocialClassesQueryHandler : IRequestHandler<GetAllSocialClassesQuery, List<SocialClassDto>>
    {
        private readonly ISocialClassRepository _repository;
        private readonly IMapper _mapper;

        public GetAllSocialClassesQueryHandler(ISocialClassRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<SocialClassDto>> Handle(GetAllSocialClassesQuery request, CancellationToken cancellationToken)
        {
            var socialClasses = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<SocialClassDto>>(socialClasses);
        }
    }

    public class GetSocialClassByIdQueryHandler : IRequestHandler<GetSocialClassByIdQuery, SocialClassDetailsDto?>
    {
        private readonly ISocialClassRepository _repository;
        private readonly IMapper _mapper;

        public GetSocialClassByIdQueryHandler(ISocialClassRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SocialClassDetailsDto?> Handle(GetSocialClassByIdQuery request, CancellationToken cancellationToken)
        {
            var socialClass = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return socialClass == null ? null : _mapper.Map<SocialClassDetailsDto>(socialClass);
        }
    }

    public class CreateSocialClassCommandHandler : IRequestHandler<CreateSocialClassCommand, SocialClassDto>
    {
        private readonly ISocialClassRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSocialClassCommandHandler> _logger;

        public CreateSocialClassCommandHandler(
            ISocialClassRepository repository,
            IWorldRepository worldRepository,
            IMapper mapper,
            ILogger<CreateSocialClassCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SocialClassDto> Handle(CreateSocialClassCommand request, CancellationToken cancellationToken)
        {
            var dto = request.SocialClassCreateDto;
            _logger.LogInformation("Creating new social class: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            var created = await _repository.CreateAsync(_mapper.Map<Entities.Social.Structure.SocialClass>(dto), cancellationToken);
            return _mapper.Map<SocialClassDto>(created);
        }
    }

    public class UpdateSocialClassCommandHandler : IRequestHandler<UpdateSocialClassCommand, SocialClassDto>
    {
        private readonly ISocialClassRepository _repository;
        private readonly IMapper _mapper;

        public UpdateSocialClassCommandHandler(ISocialClassRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SocialClassDto> Handle(UpdateSocialClassCommand request, CancellationToken cancellationToken)
        {
            var socialClass = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("SocialClass", request.Id);

            _mapper.Map(request.SocialClassUpdateDto, socialClass);
            var updated = await _repository.UpdateAsync(socialClass, cancellationToken);
            return _mapper.Map<SocialClassDto>(updated);
        }
    }

    public class DeleteSocialClassCommandHandler : IRequestHandler<DeleteSocialClassCommand, bool>
    {
        private readonly ISocialClassRepository _repository;
        private readonly ILogger<DeleteSocialClassCommandHandler> _logger;

        public DeleteSocialClassCommandHandler(ISocialClassRepository repository, ILogger<DeleteSocialClassCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteSocialClassCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting social class with ID {Id}", request.Id);

            var inUse = await _repository.CountCharactersUsingSocialClassAsync(request.Id, cancellationToken);
            if (inUse > 0)
            {
                throw new DomainValidationException(
                    $"This social class is used by {inUse} character(s). Reassign them first.");
            }

            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
