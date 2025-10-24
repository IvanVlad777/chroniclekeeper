using AutoMapper;
using ChronicleKeeper.Core.CQRS.Characters.Queries;
using ChronicleKeeper.Core.DTOs.Character;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Characters.Handlers
{
    public class GetAllCharactersQueryHandler : IRequestHandler<GetAllCharactersQuery, List<CharacterDto>>
    {
        private readonly ICharacterRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllCharactersQueryHandler> _logger;

        public GetAllCharactersQueryHandler(
            ICharacterRepository repository, 
            IMapper mapper, 
            ILogger<GetAllCharactersQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<CharacterDto>> Handle(GetAllCharactersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all characters");

            var characters = await _repository.GetAllAsync(cancellationToken);

            var result = _mapper.Map<List<CharacterDto>>(characters);
            _logger.LogInformation("Returned {Count} characters", characters.Count);

            return result;
        }
    }

    public class GetCharacterByIdQueryHandler : IRequestHandler<GetCharacterByIdQuery, CharacterDto?>
    {
        private readonly ICharacterRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCharacterByIdQueryHandler> _logger;

        public GetCharacterByIdQueryHandler(
            ICharacterRepository repository, 
            IMapper mapper, 
            ILogger<GetCharacterByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CharacterDto?> Handle(GetCharacterByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching character with ID {Id}", request.Id);

            var character = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (character == null)
            {
                _logger.LogWarning("Character with ID {Id} not found", request.Id);
                return null;
            }

            var result = _mapper.Map<CharacterDto>(character);
            _logger.LogInformation("Returned character with ID {Id}", request.Id);

            return result;
        }
    }
}
