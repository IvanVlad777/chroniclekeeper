using AutoMapper;
using ChronicleKeeper.Core.CQRS.Hobbies.Commands;
using ChronicleKeeper.Core.CQRS.Hobbies.Queries;
using ChronicleKeeper.Core.CQRS.SocialHierarchies.Handlers;
using ChronicleKeeper.Core.DTOs.Hobby;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Hobbies.Handlers
{
    public class GetAllHobbiesQueryHandler : IRequestHandler<GetAllHobbiesQuery, List<HobbyDto>>
    {
        private readonly IHobbyRepository _repository;
        private readonly IMapper _mapper;

        public GetAllHobbiesQueryHandler(IHobbyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<HobbyDto>> Handle(GetAllHobbiesQuery request, CancellationToken cancellationToken)
        {
            var hobbies = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<HobbyDto>>(hobbies);
        }
    }

    public class GetHobbyByIdQueryHandler : IRequestHandler<GetHobbyByIdQuery, HobbyDetailsDto?>
    {
        private readonly IHobbyRepository _repository;
        private readonly IMapper _mapper;

        public GetHobbyByIdQueryHandler(IHobbyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<HobbyDetailsDto?> Handle(GetHobbyByIdQuery request, CancellationToken cancellationToken)
        {
            var hobby = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return hobby == null ? null : _mapper.Map<HobbyDetailsDto>(hobby);
        }
    }

    public class CreateHobbyCommandHandler : IRequestHandler<CreateHobbyCommand, HobbyDto>
    {
        private readonly IHobbyRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateHobbyCommandHandler> _logger;

        public CreateHobbyCommandHandler(
            IHobbyRepository repository,
            IWorldRepository worldRepository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<CreateHobbyCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<HobbyDto> Handle(CreateHobbyCommand request, CancellationToken cancellationToken)
        {
            var dto = request.HobbyCreateDto;
            _logger.LogInformation("Creating new hobby: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            var hobby = _mapper.Map<Entities.Characters.CharacterInfo.Hobby>(dto);
            await TailValidation.ValidateHistoryAsync(_historyRepository, hobby.HistoryId, hobby.WorldId, cancellationToken);

            var created = await _repository.CreateAsync(hobby, cancellationToken);
            return _mapper.Map<HobbyDto>(created);
        }
    }

    public class UpdateHobbyCommandHandler : IRequestHandler<UpdateHobbyCommand, HobbyDto>
    {
        private readonly IHobbyRepository _repository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public UpdateHobbyCommandHandler(
            IHobbyRepository repository,
            IHistoryRepository historyRepository,
            IMapper mapper)
        {
            _repository = repository;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public async Task<HobbyDto> Handle(UpdateHobbyCommand request, CancellationToken cancellationToken)
        {
            var hobby = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Hobby", request.Id);

            _mapper.Map(request.HobbyUpdateDto, hobby);
            await TailValidation.ValidateHistoryAsync(_historyRepository, hobby.HistoryId, hobby.WorldId, cancellationToken);

            var updated = await _repository.UpdateAsync(hobby, cancellationToken);
            return _mapper.Map<HobbyDto>(updated);
        }
    }

    public class DeleteHobbyCommandHandler : IRequestHandler<DeleteHobbyCommand, bool>
    {
        private readonly IHobbyRepository _repository;
        private readonly ILogger<DeleteHobbyCommandHandler> _logger;

        public DeleteHobbyCommandHandler(IHobbyRepository repository, ILogger<DeleteHobbyCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteHobbyCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting hobby with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
