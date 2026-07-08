using AutoMapper;
using ChronicleKeeper.Core.CQRS.Characters.Commands;
using ChronicleKeeper.Core.DTOs.Character;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Exceptions;
using ChronicleKeeper.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChronicleKeeper.Core.CQRS.Characters.Handlers
{
    public class CreateCharacterCommandHandler : IRequestHandler<CreateCharacterCommand, CharacterDto>
    {
        private readonly ICharacterRepository _repository;
        private readonly IWorldRepository _worldRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCharacterCommandHandler> _logger;

        public CreateCharacterCommandHandler(
            ICharacterRepository repository,
            IWorldRepository worldRepository,
            IMapper mapper,
            ILogger<CreateCharacterCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CharacterDto> Handle(CreateCharacterCommand request, CancellationToken cancellationToken)
        {
            var dto = request.CharacterCreateDto;
            _logger.LogInformation("Creating new character: {Name}", dto.Name);

            if (!await _worldRepository.ExistsAsync(dto.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"World with ID {dto.WorldId} does not exist.");
            }

            var character = _mapper.Map<Character>(dto);
            await CharacterValidation.ValidateReferencesAsync(_repository, character, cancellationToken);

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

            // Lean fetch — update mijenja samo skalarna polja, graf nije potreban
            var character = await _repository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new EntityNotFoundException("Character", request.Id);

            _mapper.Map(request.CharacterUpdateDto, character);
            await CharacterValidation.ValidateReferencesAsync(_repository, character, cancellationToken);

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

            var deleted = await _repository.DeleteAsync(request.Id, cancellationToken);
            if (!deleted)
            {
                _logger.LogWarning("Character with ID {Id} not found for deletion", request.Id);
            }
            return deleted;
        }
    }

    internal static class CharacterValidation
    {
        /// <summary>
        /// Sve reference lika (roditelji, vrsta, rasa) moraju postojati U ISTOM SVIJETU —
        /// cross-world reference bi kasnije blokirale brisanje tuđeg svijeta (Restrict FK).
        /// Invarijanta rase: ako je zadana samo rasa, vrsta se izvodi iz nje;
        /// ako su zadane obje, moraju se slagati.
        /// </summary>
        public static async Task ValidateReferencesAsync(
            ICharacterRepository repository, Character character, CancellationToken cancellationToken)
        {
            if (character.FatherId is int fatherId)
            {
                if (fatherId == character.Id)
                {
                    throw new DomainValidationException("A character cannot be their own father.");
                }
                if (!await repository.ExistsInWorldAsync(fatherId, character.WorldId, cancellationToken))
                {
                    throw new DomainValidationException($"Father character with ID {fatherId} does not exist in this world.");
                }
            }

            if (character.MotherId is int motherId)
            {
                if (motherId == character.Id)
                {
                    throw new DomainValidationException("A character cannot be their own mother.");
                }
                if (!await repository.ExistsInWorldAsync(motherId, character.WorldId, cancellationToken))
                {
                    throw new DomainValidationException($"Mother character with ID {motherId} does not exist in this world.");
                }
            }

            if (character.RaceId is int raceId)
            {
                var raceSpeciesId = await repository.GetSpeciesIdForRaceAsync(raceId, character.WorldId, cancellationToken);
                if (raceSpeciesId == null)
                {
                    throw new DomainValidationException($"Race with ID {raceId} does not exist in this world.");
                }

                if (character.SapientSpeciesId == null)
                {
                    character.SapientSpeciesId = raceSpeciesId;
                }
                else if (character.SapientSpeciesId != raceSpeciesId)
                {
                    throw new DomainValidationException(
                        $"Race with ID {raceId} does not belong to species with ID {character.SapientSpeciesId}.");
                }
            }
            else if (character.SapientSpeciesId is int speciesId
                && !await repository.SpeciesExistsInWorldAsync(speciesId, character.WorldId, cancellationToken))
            {
                throw new DomainValidationException($"Species with ID {speciesId} does not exist in this world.");
            }
        }
    }
}
