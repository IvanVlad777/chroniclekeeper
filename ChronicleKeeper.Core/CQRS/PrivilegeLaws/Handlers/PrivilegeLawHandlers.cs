using AutoMapper;
using ChronicleKeeper.Core.CQRS.PrivilegeLaws.Commands;
using ChronicleKeeper.Core.CQRS.PrivilegeLaws.Queries;
using ChronicleKeeper.Core.CQRS.SocialHierarchies.Handlers;
using ChronicleKeeper.Core.DTOs.PrivilegeLaw;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.PrivilegeLaws.Handlers
{
    public class GetAllPrivilegeLawsQueryHandler : IRequestHandler<GetAllPrivilegeLawsQuery, List<PrivilegeLawDto>>
    {
        private readonly IPrivilegeLawRepository _repository;
        private readonly IMapper _mapper;

        public GetAllPrivilegeLawsQueryHandler(IPrivilegeLawRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PrivilegeLawDto>> Handle(GetAllPrivilegeLawsQuery request, CancellationToken cancellationToken)
        {
            var laws = await _repository.GetAllAsync(request.WorldId, request.SocialClassId, cancellationToken);
            return _mapper.Map<List<PrivilegeLawDto>>(laws);
        }
    }

    public class GetPrivilegeLawByIdQueryHandler : IRequestHandler<GetPrivilegeLawByIdQuery, PrivilegeLawDto?>
    {
        private readonly IPrivilegeLawRepository _repository;
        private readonly IMapper _mapper;

        public GetPrivilegeLawByIdQueryHandler(IPrivilegeLawRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PrivilegeLawDto?> Handle(GetPrivilegeLawByIdQuery request, CancellationToken cancellationToken)
        {
            var law = await _repository.FindByIdAsync(request.Id, cancellationToken);
            return law == null ? null : _mapper.Map<PrivilegeLawDto>(law);
        }
    }

    public class CreatePrivilegeLawCommandHandler : IRequestHandler<CreatePrivilegeLawCommand, PrivilegeLawDto>
    {
        private readonly IPrivilegeLawRepository _repository;
        private readonly ISocialClassRepository _socialClassRepository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreatePrivilegeLawCommandHandler> _logger;

        public CreatePrivilegeLawCommandHandler(
            IPrivilegeLawRepository repository,
            ISocialClassRepository socialClassRepository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<CreatePrivilegeLawCommandHandler> logger)
        {
            _repository = repository;
            _socialClassRepository = socialClassRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PrivilegeLawDto> Handle(CreatePrivilegeLawCommand request, CancellationToken cancellationToken)
        {
            var dto = request.PrivilegeLawCreateDto;
            _logger.LogInformation("Creating new privilege law: {Name}", dto.Name);

            var socialClass = await _socialClassRepository.FindByIdAsync(dto.SocialClassId, cancellationToken)
                ?? throw new DomainValidationException($"Social class with ID {dto.SocialClassId} does not exist.");

            var law = _mapper.Map<Entities.Social.Structure.PrivilegeLaw>(dto);
            law.WorldId = socialClass.WorldId; // the law's world always = its class's world

            await TailValidation.ValidateHistoryAsync(_historyRepository, law.HistoryId, law.WorldId, cancellationToken);

            var created = await _repository.CreateAsync(law, cancellationToken);
            return _mapper.Map<PrivilegeLawDto>(created);
        }
    }

    public class UpdatePrivilegeLawCommandHandler : IRequestHandler<UpdatePrivilegeLawCommand, PrivilegeLawDto>
    {
        private readonly IPrivilegeLawRepository _repository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;

        public UpdatePrivilegeLawCommandHandler(
            IPrivilegeLawRepository repository,
            IHistoryRepository historyRepository,
            IMapper mapper)
        {
            _repository = repository;
            _historyRepository = historyRepository;
            _mapper = mapper;
        }

        public async Task<PrivilegeLawDto> Handle(UpdatePrivilegeLawCommand request, CancellationToken cancellationToken)
        {
            var law = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("PrivilegeLaw", request.Id);

            _mapper.Map(request.PrivilegeLawUpdateDto, law);
            await TailValidation.ValidateHistoryAsync(_historyRepository, law.HistoryId, law.WorldId, cancellationToken);

            var updated = await _repository.UpdateAsync(law, cancellationToken);
            return _mapper.Map<PrivilegeLawDto>(updated);
        }
    }

    public class DeletePrivilegeLawCommandHandler : IRequestHandler<DeletePrivilegeLawCommand, bool>
    {
        private readonly IPrivilegeLawRepository _repository;
        private readonly ILogger<DeletePrivilegeLawCommandHandler> _logger;

        public DeletePrivilegeLawCommandHandler(IPrivilegeLawRepository repository, ILogger<DeletePrivilegeLawCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeletePrivilegeLawCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting privilege law with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
