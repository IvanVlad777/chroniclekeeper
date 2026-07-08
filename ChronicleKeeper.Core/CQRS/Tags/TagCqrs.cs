using AutoMapper;
using ChronicleKeeper.Core.DTOs.Tag;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using TagEntity = ChronicleKeeper.Core.Entities.Tags.Tag;

namespace ChronicleKeeper.Core.CQRS.Tags
{
    // ----- Queries -----

    public class GetAllTagsQuery : IRequest<List<TagDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetTagByIdQuery : IRequest<TagDto?>
    {
        public int Id { get; set; }
    }

    // ----- Commands -----

    public class CreateTagCommand : IRequest<TagDto>
    {
        public TagCreateDto TagCreateDto { get; set; } = new();
    }

    public class UpdateTagCommand : IRequest<TagDto>
    {
        public int Id { get; set; }
        public TagUpdateDto TagUpdateDto { get; set; } = new();
    }

    public class DeleteTagCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class AttachTagCommand : IRequest<Unit>
    {
        public int TagId { get; set; }
        public TagAttachDto AttachDto { get; set; } = new();
    }

    public class DetachTagCommand : IRequest<bool>
    {
        public int TagId { get; set; }
        public TagTargetType TargetType { get; set; }
        public int TargetId { get; set; }
    }

    // ----- Handlers -----

    public class GetAllTagsQueryHandler : IRequestHandler<GetAllTagsQuery, List<TagDto>>
    {
        private readonly ITagRepository _repository;
        private readonly IMapper _mapper;

        public GetAllTagsQueryHandler(ITagRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<TagDto>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
        {
            var tags = await _repository.GetAllAsync(request.WorldId, cancellationToken);
            return _mapper.Map<List<TagDto>>(tags);
        }
    }

    public class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, TagDto?>
    {
        private readonly ITagRepository _repository;
        private readonly IMapper _mapper;

        public GetTagByIdQueryHandler(ITagRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TagDto?> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
        {
            var tag = await _repository.FindByIdAsync(request.Id, cancellationToken);
            return tag == null ? null : _mapper.Map<TagDto>(tag);
        }
    }

    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, TagDto>
    {
        private readonly ITagRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTagCommandHandler> _logger;

        public CreateTagCommandHandler(
            ITagRepository repository,
            IWorldRepository worldRepository,
            IMapper mapper,
            ILogger<CreateTagCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TagDto> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var dto = request.TagCreateDto;
            _logger.LogInformation("Creating new tag: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }
            if (await _repository.NameExistsInWorldAsync(dto.Name, dto.WorldId, null, cancellationToken))
            {
                throw new DomainValidationException($"A tag named '{dto.Name}' already exists in this world.");
            }

            var created = await _repository.CreateAsync(_mapper.Map<TagEntity>(dto), cancellationToken);
            return _mapper.Map<TagDto>(created);
        }
    }

    public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand, TagDto>
    {
        private readonly ITagRepository _repository;
        private readonly IMapper _mapper;

        public UpdateTagCommandHandler(ITagRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TagDto> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Tag", request.Id);

            if (await _repository.NameExistsInWorldAsync(request.TagUpdateDto.Name, tag.WorldId, tag.Id, cancellationToken))
            {
                throw new DomainValidationException($"A tag named '{request.TagUpdateDto.Name}' already exists in this world.");
            }

            _mapper.Map(request.TagUpdateDto, tag);
            var updated = await _repository.UpdateAsync(tag, cancellationToken);
            return _mapper.Map<TagDto>(updated);
        }
    }

    public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand, bool>
    {
        private readonly ITagRepository _repository;

        public DeleteTagCommandHandler(ITagRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }

    public class AttachTagCommandHandler : IRequestHandler<AttachTagCommand, Unit>
    {
        private readonly ITagRepository _repository;
        private readonly ILogger<AttachTagCommandHandler> _logger;

        public AttachTagCommandHandler(ITagRepository repository, ILogger<AttachTagCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Unit> Handle(AttachTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _repository.FindByIdAsync(request.TagId, cancellationToken)
                ?? throw new EntityNotFoundException("Tag", request.TagId);

            var dto = request.AttachDto;
            if (!await _repository.TargetExistsInWorldAsync(dto.TargetType, dto.TargetId, tag.WorldId, cancellationToken))
            {
                throw new DomainValidationException(
                    $"{dto.TargetType} with ID {dto.TargetId} does not exist in this world.");
            }
            if (await _repository.IsAttachedAsync(request.TagId, dto.TargetType, dto.TargetId, cancellationToken))
            {
                throw new DomainValidationException("This tag is already attached to that entity.");
            }

            await _repository.AttachAsync(request.TagId, dto.TargetType, dto.TargetId, cancellationToken);
            _logger.LogInformation("Attached tag {TagId} to {TargetType} {TargetId}", request.TagId, dto.TargetType, dto.TargetId);
            return Unit.Value;
        }
    }

    public class DetachTagCommandHandler : IRequestHandler<DetachTagCommand, bool>
    {
        private readonly ITagRepository _repository;

        public DetachTagCommandHandler(ITagRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DetachTagCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DetachAsync(request.TagId, request.TargetType, request.TargetId, cancellationToken);
        }
    }
}
