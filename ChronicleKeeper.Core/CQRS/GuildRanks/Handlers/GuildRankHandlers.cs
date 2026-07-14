using AutoMapper;
using ChronicleKeeper.Core.CQRS.GuildRanks.Commands;
using ChronicleKeeper.Core.CQRS.GuildRanks.Queries;
using ChronicleKeeper.Core.DTOs.GuildRank;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.GuildRanks.Handlers
{
    public class GetAllGuildRanksQueryHandler : IRequestHandler<GetAllGuildRanksQuery, List<GuildRankDto>>
    {
        private readonly IGuildRankRepository _repository;
        private readonly IMapper _mapper;

        public GetAllGuildRanksQueryHandler(IGuildRankRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<GuildRankDto>> Handle(GetAllGuildRanksQuery request, CancellationToken cancellationToken)
        {
            var guildRanks = await _repository.GetAllAsync(request.WorldId, request.GuildId, cancellationToken);
            return _mapper.Map<List<GuildRankDto>>(guildRanks);
        }
    }

    public class GetGuildRankByIdQueryHandler : IRequestHandler<GetGuildRankByIdQuery, GuildRankDto?>
    {
        private readonly IGuildRankRepository _repository;
        private readonly IMapper _mapper;

        public GetGuildRankByIdQueryHandler(IGuildRankRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GuildRankDto?> Handle(GetGuildRankByIdQuery request, CancellationToken cancellationToken)
        {
            var guildRank = await _repository.FindByIdAsync(request.Id, cancellationToken);
            return guildRank == null ? null : _mapper.Map<GuildRankDto>(guildRank);
        }
    }

    public class CreateGuildRankCommandHandler : IRequestHandler<CreateGuildRankCommand, GuildRankDto>
    {
        private readonly IGuildRankRepository _repository;
        private readonly IGuildRepository _guildRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateGuildRankCommandHandler> _logger;

        public CreateGuildRankCommandHandler(
            IGuildRankRepository repository,
            IGuildRepository guildRepository,
            IMapper mapper,
            ILogger<CreateGuildRankCommandHandler> logger)
        {
            _repository = repository;
            _guildRepository = guildRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GuildRankDto> Handle(CreateGuildRankCommand request, CancellationToken cancellationToken)
        {
            var dto = request.GuildRankCreateDto;
            _logger.LogInformation("Creating new guild rank: {Name}", dto.Name);

            var guild = await _guildRepository.FindByIdAsync(dto.GuildId, cancellationToken)
                ?? throw new DomainValidationException($"Guild with ID {dto.GuildId} does not exist.");

            var guildRank = _mapper.Map<Entities.Social.Economy.GuildRank>(dto);
            guildRank.WorldId = guild.WorldId; // svijet ranga uvijek = svijet ceha

            var created = await _repository.CreateAsync(guildRank, cancellationToken);
            return _mapper.Map<GuildRankDto>(created);
        }
    }

    public class UpdateGuildRankCommandHandler : IRequestHandler<UpdateGuildRankCommand, GuildRankDto>
    {
        private readonly IGuildRankRepository _repository;
        private readonly IMapper _mapper;

        public UpdateGuildRankCommandHandler(IGuildRankRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GuildRankDto> Handle(UpdateGuildRankCommand request, CancellationToken cancellationToken)
        {
            var guildRank = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("GuildRank", request.Id);

            _mapper.Map(request.GuildRankUpdateDto, guildRank);
            var updated = await _repository.UpdateAsync(guildRank, cancellationToken);
            return _mapper.Map<GuildRankDto>(updated);
        }
    }

    public class DeleteGuildRankCommandHandler : IRequestHandler<DeleteGuildRankCommand, bool>
    {
        private readonly IGuildRankRepository _repository;
        private readonly ILogger<DeleteGuildRankCommandHandler> _logger;

        public DeleteGuildRankCommandHandler(IGuildRankRepository repository, ILogger<DeleteGuildRankCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteGuildRankCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting guild rank with ID {Id}", request.Id);
            return await _repository.DeleteAsync(request.Id, cancellationToken);
        }
    }
}
