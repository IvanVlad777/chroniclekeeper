using AutoMapper;
using ChronicleKeeper.Core.CQRS.Characters.Commands;
using ChronicleKeeper.Core.DTOs.Character;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Characters.Handlers
{
    public class CreateCharacterCommandHandler : IRequestHandler<CreateCharacterCommand, CharacterDto>
    {
        private readonly ICharacterRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCharacterCommandHandler> _logger;

        public CreateCharacterCommandHandler(
            ICharacterRepository repository, 
            IMapper mapper, 
            ILogger<CreateCharacterCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CharacterDto> Handle(CreateCharacterCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating new character: {Name}", request.CharacterCreateDto.Name);

            var character = _mapper.Map<Character>(request.CharacterCreateDto);
            character.CreatedAt = DateTime.UtcNow;
            character.UpdatedAt = DateTime.UtcNow;

            var createdCharacter = await _repository.CreateAsync(character, cancellationToken);

            var result = _mapper.Map<CharacterDto>(createdCharacter);
            _logger.LogInformation("Created character with ID {Id}", createdCharacter.Id);

            return result;
        }
    }

    public class UpdateCharacterCommandHandler : IRequestHandler<UpdateCharacterCommand, CharacterDto>
    {
        private readonly ICharacterRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCharacterCommandHandler> _logger;

        public UpdateCharacterCommandHandler(
            ICharacterRepository repository, 
            IMapper mapper, 
            ILogger<UpdateCharacterCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CharacterDto> Handle(UpdateCharacterCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating character with ID {Id}", request.Id);

            var character = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (character == null)
            {
                _logger.LogWarning("Character with ID {Id} not found for update", request.Id);
                throw new InvalidOperationException($"Character with ID {request.Id} not found");
            }

            _mapper.Map(request.CharacterUpdateDto, character);
            character.UpdatedAt = DateTime.UtcNow;

            var updatedCharacter = await _repository.UpdateAsync(character, cancellationToken);

            var result = _mapper.Map<CharacterDto>(updatedCharacter);
            _logger.LogInformation("Updated character with ID {Id}", request.Id);

            return result;
        }
    }

    public class DeleteCharacterCommandHandler : IRequestHandler<DeleteCharacterCommand, bool>
    {
        private readonly ICharacterRepository _repository;
        private readonly ILogger<DeleteCharacterCommandHandler> _logger;

        public DeleteCharacterCommandHandler(
            ICharacterRepository repository, 
            ILogger<DeleteCharacterCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteCharacterCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting character with ID {Id}", request.Id);

            var character = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (character == null)
            {
                _logger.LogWarning("Character with ID {Id} not found for deletion", request.Id);
                return false;
            }

            await _repository.DeleteAsync(request.Id, cancellationToken);

            _logger.LogInformation("Deleted character with ID {Id}", request.Id);
            return true;
        }
    }
}
