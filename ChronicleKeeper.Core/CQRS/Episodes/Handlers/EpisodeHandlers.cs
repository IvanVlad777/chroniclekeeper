using AutoMapper;
using ChronicleKeeper.Core.CQRS.Episodes.Commands;
using ChronicleKeeper.Core.CQRS.Episodes.Queries;
using ChronicleKeeper.Core.DTOs.Episode;
using ChronicleKeeper.Core.Entities.Content.Movie;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Episodes.Handlers
{
    public class GetAllEpisodesQueryHandler : IRequestHandler<GetAllEpisodesQuery, List<EpisodeDto>>
    {
        private readonly IEpisodeRepository _repository;
        private readonly IMapper _mapper;

        public GetAllEpisodesQueryHandler(IEpisodeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<EpisodeDto>> Handle(GetAllEpisodesQuery request, CancellationToken cancellationToken)
        {
            var episodes = await _repository.GetAllAsync(request.WorldId, request.SeriesId, cancellationToken);
            return _mapper.Map<List<EpisodeDto>>(episodes);
        }
    }

    public class GetEpisodeByIdQueryHandler : IRequestHandler<GetEpisodeByIdQuery, EpisodeDto?>
    {
        private readonly IEpisodeRepository _repository;
        private readonly IMapper _mapper;

        public GetEpisodeByIdQueryHandler(IEpisodeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EpisodeDto?> Handle(GetEpisodeByIdQuery request, CancellationToken cancellationToken)
        {
            var episode = await _repository.FindByIdAsync(request.Id, cancellationToken);
            return episode == null ? null : _mapper.Map<EpisodeDto>(episode);
        }
    }

    public class CreateEpisodeCommandHandler : IRequestHandler<CreateEpisodeCommand, EpisodeDto>
    {
        private readonly IEpisodeRepository _repository;
        private readonly IContentRepository _contentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateEpisodeCommandHandler> _logger;

        public CreateEpisodeCommandHandler(
            IEpisodeRepository repository,
            IContentRepository contentRepository,
            IMapper mapper,
            ILogger<CreateEpisodeCommandHandler> logger)
        {
            _repository = repository;
            _contentRepository = contentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<EpisodeDto> Handle(CreateEpisodeCommand request, CancellationToken cancellationToken)
        {
            var dto = request.EpisodeCreateDto;
            _logger.LogInformation("Creating new episode: {Name}", dto.Name);

            var series = await _contentRepository.FindByIdAsync(dto.SeriesId, cancellationToken);
            if (series is not Series)
            {
                throw new DomainValidationException($"Series with ID {dto.SeriesId} does not exist.");
            }

            var episode = _mapper.Map<Episode>(dto);
            episode.WorldId = series.WorldId; // svijet epizode uvijek = svijet serije

            var created = await _repository.CreateAsync(episode, cancellationToken);
            return _mapper.Map<EpisodeDto>(created);
        }
    }

    public class UpdateEpisodeCommandHandler : IRequestHandler<UpdateEpisodeCommand, EpisodeDto>
    {
        private readonly IEpisodeRepository _repository;
        private readonly IMapper _mapper;

        public UpdateEpisodeCommandHandler(IEpisodeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EpisodeDto> Handle(UpdateEpisodeCommand request, CancellationToken cancellationToken)
        {
            var episode = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Episode", request.Id);

            _mapper.Map(request.EpisodeUpdateDto, episode);
            var updated = await _repository.UpdateAsync(episode, cancellationToken);
            return _mapper.Map<EpisodeDto>(updated);
        }
    }

    public class DeleteEpisodeCommandHandler : IRequestHandler<DeleteEpisodeCommand, bool>
    {
        private readonly IEpisodeRepository _repository;
        private readonly ILogger<DeleteEpisodeCommandHandler> _logger;

        public DeleteEpisodeCommandHandler(IEpisodeRepository repository, ILogger<DeleteEpisodeCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteEpisodeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting episode with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
