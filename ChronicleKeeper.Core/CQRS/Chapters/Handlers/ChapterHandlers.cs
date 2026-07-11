using AutoMapper;
using ChronicleKeeper.Core.CQRS.Chapters.Commands;
using ChronicleKeeper.Core.CQRS.Chapters.Queries;
using ChronicleKeeper.Core.DTOs.Chapter;
using ChronicleKeeper.Core.Entities.Content.Book;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Chapters.Handlers
{
    public class GetAllChaptersQueryHandler : IRequestHandler<GetAllChaptersQuery, List<ChapterDto>>
    {
        private readonly IChapterRepository _repository;
        private readonly IMapper _mapper;

        public GetAllChaptersQueryHandler(IChapterRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ChapterDto>> Handle(GetAllChaptersQuery request, CancellationToken cancellationToken)
        {
            var chapters = await _repository.GetAllAsync(request.WorldId, request.BookId, cancellationToken);
            return _mapper.Map<List<ChapterDto>>(chapters);
        }
    }

    public class GetChapterByIdQueryHandler : IRequestHandler<GetChapterByIdQuery, ChapterDto?>
    {
        private readonly IChapterRepository _repository;
        private readonly IMapper _mapper;

        public GetChapterByIdQueryHandler(IChapterRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ChapterDto?> Handle(GetChapterByIdQuery request, CancellationToken cancellationToken)
        {
            var chapter = await _repository.FindByIdAsync(request.Id, cancellationToken);
            return chapter == null ? null : _mapper.Map<ChapterDto>(chapter);
        }
    }

    public class CreateChapterCommandHandler : IRequestHandler<CreateChapterCommand, ChapterDto>
    {
        private readonly IChapterRepository _repository;
        private readonly IContentRepository _contentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateChapterCommandHandler> _logger;

        public CreateChapterCommandHandler(
            IChapterRepository repository,
            IContentRepository contentRepository,
            IMapper mapper,
            ILogger<CreateChapterCommandHandler> logger)
        {
            _repository = repository;
            _contentRepository = contentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ChapterDto> Handle(CreateChapterCommand request, CancellationToken cancellationToken)
        {
            var dto = request.ChapterCreateDto;
            _logger.LogInformation("Creating new chapter: {Name}", dto.Name);

            var book = await _contentRepository.FindByIdAsync(dto.BookId, cancellationToken);
            if (book is not Book)
            {
                throw new DomainValidationException($"Book with ID {dto.BookId} does not exist.");
            }

            var chapter = _mapper.Map<Chapter>(dto);
            chapter.WorldId = book.WorldId; // svijet poglavlja uvijek = svijet knjige

            var created = await _repository.CreateAsync(chapter, cancellationToken);
            return _mapper.Map<ChapterDto>(created);
        }
    }

    public class UpdateChapterCommandHandler : IRequestHandler<UpdateChapterCommand, ChapterDto>
    {
        private readonly IChapterRepository _repository;
        private readonly IMapper _mapper;

        public UpdateChapterCommandHandler(IChapterRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ChapterDto> Handle(UpdateChapterCommand request, CancellationToken cancellationToken)
        {
            var chapter = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Chapter", request.Id);

            _mapper.Map(request.ChapterUpdateDto, chapter);
            var updated = await _repository.UpdateAsync(chapter, cancellationToken);
            return _mapper.Map<ChapterDto>(updated);
        }
    }

    public class DeleteChapterCommandHandler : IRequestHandler<DeleteChapterCommand, bool>
    {
        private readonly IChapterRepository _repository;
        private readonly ILogger<DeleteChapterCommandHandler> _logger;

        public DeleteChapterCommandHandler(IChapterRepository repository, ILogger<DeleteChapterCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteChapterCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting chapter with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
