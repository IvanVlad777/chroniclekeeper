using AutoMapper;
using ChronicleKeeper.Core.CQRS.Contents.Commands;
using ChronicleKeeper.Core.CQRS.Contents.Queries;
using ChronicleKeeper.Core.DTOs.Content;
using ChronicleKeeper.Core.Entities.Content;
using ChronicleKeeper.Core.Entities.Content.Article;
using ChronicleKeeper.Core.Entities.Content.Book;
using ChronicleKeeper.Core.Entities.Content.Movie;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Contents.Handlers
{
    public class GetAllContentsQueryHandler : IRequestHandler<GetAllContentsQuery, List<ContentDto>>
    {
        private readonly IContentRepository _repository;
        private readonly IMapper _mapper;

        public GetAllContentsQueryHandler(IContentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ContentDto>> Handle(GetAllContentsQuery request, CancellationToken cancellationToken)
        {
            var contents = await _repository.GetAllAsync(request.WorldId, request.Type, cancellationToken);
            return _mapper.Map<List<ContentDto>>(contents);
        }
    }

    public class GetContentByIdQueryHandler : IRequestHandler<GetContentByIdQuery, ContentDetailsDto?>
    {
        private readonly IContentRepository _repository;
        private readonly IMapper _mapper;

        public GetContentByIdQueryHandler(IContentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ContentDetailsDto?> Handle(GetContentByIdQuery request, CancellationToken cancellationToken)
        {
            var content = await _repository.GetByIdAsync(request.Id, cancellationToken);
            return content == null ? null : _mapper.Map<ContentDetailsDto>(content);
        }
    }

    public class CreateContentCommandHandler : IRequestHandler<CreateContentCommand, ContentDto>
    {
        private static readonly string[] KnownTypes = { "Article", "Book", "Comic", "Movie", "Series" };

        private readonly IContentRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateContentCommandHandler> _logger;

        public CreateContentCommandHandler(
            IContentRepository repository,
            IWorldRepository worldRepository,
            IMapper mapper,
            ILogger<CreateContentCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ContentDto> Handle(CreateContentCommand request, CancellationToken cancellationToken)
        {
            var dto = request.ContentCreateDto;
            _logger.LogInformation("Creating new content: {Name} ({Type})", dto.Name, dto.Type);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            if (!KnownTypes.Contains(dto.Type))
            {
                throw new DomainValidationException($"Unknown content type '{dto.Type}'. Expected one of: {string.Join(", ", KnownTypes)}.");
            }

            Content content = dto.Type switch
            {
                "Article" => new Article
                {
                    Source = dto.Source ?? string.Empty,
                    PublishDate = dto.PublishDate ?? default
                },
                "Book" => new Book
                {
                    Author = dto.Author ?? string.Empty,
                    ReleaseDate = dto.ReleaseDate ?? default
                },
                "Comic" => new Comic
                {
                    Author = dto.Author ?? string.Empty,
                    IssueNumber = dto.IssueNumber ?? 0
                },
                "Movie" => new Movie
                {
                    Director = dto.Director ?? string.Empty,
                    ReleaseDate = dto.ReleaseDate ?? default,
                    DurationMinutes = dto.DurationMinutes ?? 0,
                    PrequelId = dto.PrequelId
                },
                "Series" => new Series
                {
                    Creator = dto.Creator ?? string.Empty,
                    Seasons = dto.Seasons ?? 0
                },
                _ => throw new DomainValidationException($"Unknown content type '{dto.Type}'.")
            };

            content.Name = dto.Name;
            content.Description = dto.Description;
            content.WorldId = dto.WorldId;

            if (content is Movie { PrequelId: int prequelId })
            {
                var prequel = await _repository.FindByIdAsync(prequelId, cancellationToken);
                if (prequel is not Movie prequelMovie || prequelMovie.WorldId != dto.WorldId)
                {
                    throw new DomainValidationException($"Movie with ID {prequelId} does not exist in this world.");
                }
                // No self-reference check needed on create — the new Content has no Id yet.
            }

            var created = await _repository.CreateAsync(content, cancellationToken);
            return _mapper.Map<ContentDto>(created);
        }
    }

    public class UpdateContentCommandHandler : IRequestHandler<UpdateContentCommand, ContentDto>
    {
        private readonly IContentRepository _repository;
        private readonly IMapper _mapper;

        public UpdateContentCommandHandler(IContentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ContentDto> Handle(UpdateContentCommand request, CancellationToken cancellationToken)
        {
            var content = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Content", request.Id);

            var dto = request.ContentUpdateDto;
            content.Name = dto.Name;
            content.Description = dto.Description;

            switch (content)
            {
                case Article article:
                    article.Source = dto.Source ?? string.Empty;
                    article.PublishDate = dto.PublishDate ?? default;
                    break;

                case Book book:
                    book.Author = dto.Author ?? string.Empty;
                    book.ReleaseDate = dto.ReleaseDate ?? default;
                    break;

                case Comic comic:
                    comic.Author = dto.Author ?? string.Empty;
                    comic.IssueNumber = dto.IssueNumber ?? 0;
                    break;

                case Movie movie:
                    if (dto.PrequelId is int prequelId)
                    {
                        if (prequelId == movie.Id)
                        {
                            throw new DomainValidationException("A movie cannot be its own prequel.");
                        }

                        var prequel = await _repository.FindByIdAsync(prequelId, cancellationToken);
                        if (prequel is not Movie prequelMovie || prequelMovie.WorldId != movie.WorldId)
                        {
                            throw new DomainValidationException($"Movie with ID {prequelId} does not exist in this world.");
                        }
                    }

                    movie.Director = dto.Director ?? string.Empty;
                    movie.ReleaseDate = dto.ReleaseDate ?? default;
                    movie.DurationMinutes = dto.DurationMinutes ?? 0;
                    movie.PrequelId = dto.PrequelId;
                    break;

                case Series series:
                    series.Creator = dto.Creator ?? string.Empty;
                    series.Seasons = dto.Seasons ?? 0;
                    break;
            }

            var updated = await _repository.UpdateAsync(content, cancellationToken);
            return _mapper.Map<ContentDto>(updated);
        }
    }

    public class DeleteContentCommandHandler : IRequestHandler<DeleteContentCommand, bool>
    {
        private readonly IContentRepository _repository;
        private readonly ILogger<DeleteContentCommandHandler> _logger;

        public DeleteContentCommandHandler(IContentRepository repository, ILogger<DeleteContentCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteContentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting content with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
