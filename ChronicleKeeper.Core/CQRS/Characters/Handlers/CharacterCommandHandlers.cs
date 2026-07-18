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
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCharacterCommandHandler> _logger;

        public CreateCharacterCommandHandler(
            ICharacterRepository repository,
            IWorldRepository worldRepository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<CreateCharacterCommandHandler> logger)
        {
            _repository = repository;
            _worldRepository = worldRepository;
            _historyRepository = historyRepository;
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
            await CharacterValidation.ValidateReferencesAsync(_repository, _historyRepository, character, cancellationToken);

            var createdCharacter = await _repository.CreateAsync(character, cancellationToken);

            var result = _mapper.Map<CharacterDto>(createdCharacter);
            _logger.LogInformation("Created character with ID {Id}", createdCharacter.Id);

            return result;
        }
    }

    public class UpdateCharacterCommandHandler : IRequestHandler<UpdateCharacterCommand, CharacterDto>
    {
        private readonly ICharacterRepository _repository;
        private readonly IHistoryRepository _historyRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCharacterCommandHandler> _logger;

        public UpdateCharacterCommandHandler(
            ICharacterRepository repository,
            IHistoryRepository historyRepository,
            IMapper mapper,
            ILogger<UpdateCharacterCommandHandler> logger)
        {
            _repository = repository;
            _historyRepository = historyRepository;
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
            await CharacterValidation.ValidateReferencesAsync(_repository, _historyRepository, character, cancellationToken);

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

    public class AddCharacterRelationshipCommandHandler : IRequestHandler<AddCharacterRelationshipCommand, CharacterRelationshipDto>
    {
        private readonly ICharacterRepository _repository;
        private readonly ILogger<AddCharacterRelationshipCommandHandler> _logger;

        public AddCharacterRelationshipCommandHandler(
            ICharacterRepository repository,
            ILogger<AddCharacterRelationshipCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<CharacterRelationshipDto> Handle(AddCharacterRelationshipCommand request, CancellationToken cancellationToken)
        {
            var character = await _repository.FindByIdAsync(request.CharacterId, cancellationToken)
                ?? throw new EntityNotFoundException("Character", request.CharacterId);

            var dto = request.RelationshipDto;
            if (dto.RelatedCharacterId == character.Id)
            {
                throw new DomainValidationException("A character cannot have a relationship with themselves.");
            }
            if (!await _repository.ExistsInWorldAsync(dto.RelatedCharacterId, character.WorldId, cancellationToken))
            {
                throw new DomainValidationException(
                    $"Related character with ID {dto.RelatedCharacterId} does not exist in this world.");
            }
            if (await _repository.RelationshipExistsAsync(character.Id, dto.RelatedCharacterId, dto.Type, cancellationToken))
            {
                throw new DomainValidationException(
                    $"A '{dto.Type}' relationship to that character already exists.");
            }

            var relationship = new Entities.Characters.CharacterInfo.CharacterRelationship
            {
                CharacterId = character.Id,
                RelatedCharacterId = dto.RelatedCharacterId,
                Type = dto.Type,
                Description = dto.Description,
                IsSecret = dto.IsSecret
            };
            var created = await _repository.AddRelationshipAsync(relationship, cancellationToken);

            _logger.LogInformation("Added {Type} relationship {Id} for character {CharacterId}",
                dto.Type, created.Id, character.Id);

            return new CharacterRelationshipDto
            {
                Id = created.Id,
                RelatedCharacterId = created.RelatedCharacterId,
                RelatedCharacterName = created.RelatedCharacter?.Name ?? string.Empty,
                Type = created.Type.ToString(),
                Description = created.Description,
                IsSecret = created.IsSecret
            };
        }
    }

    public class RemoveCharacterRelationshipCommandHandler : IRequestHandler<RemoveCharacterRelationshipCommand, bool>
    {
        private readonly ICharacterRepository _repository;

        public RemoveCharacterRelationshipCommandHandler(ICharacterRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(RemoveCharacterRelationshipCommand request, CancellationToken cancellationToken)
        {
            return await _repository.RemoveRelationshipAsync(request.CharacterId, request.RelationshipId, cancellationToken);
        }
    }

    public class AddCharacterAbilityCommandHandler : IRequestHandler<AddCharacterAbilityCommand, bool>
    {
        private readonly ICharacterRepository _repository;
        private readonly IAbilityRepository _abilityRepository;

        public AddCharacterAbilityCommandHandler(ICharacterRepository repository, IAbilityRepository abilityRepository)
        {
            _repository = repository;
            _abilityRepository = abilityRepository;
        }

        public async Task<bool> Handle(AddCharacterAbilityCommand request, CancellationToken cancellationToken)
        {
            var character = await _repository.FindByIdAsync(request.CharacterId, cancellationToken)
                ?? throw new EntityNotFoundException("Character", request.CharacterId);

            var ability = await _abilityRepository.FindByIdAsync(request.AbilityId, cancellationToken)
                ?? throw new DomainValidationException($"Ability with ID {request.AbilityId} does not exist.");
            if (ability.WorldId != character.WorldId)
            {
                throw new DomainValidationException($"Ability with ID {request.AbilityId} does not belong to this world.");
            }

            if (await _repository.IsAbilityLinkedAsync(request.CharacterId, request.AbilityId, cancellationToken))
            {
                throw new DomainValidationException("This ability is already linked to the character.");
            }

            await _repository.AddAbilityAsync(request.CharacterId, request.AbilityId, cancellationToken);
            return true;
        }
    }

    public class RemoveCharacterAbilityCommandHandler : IRequestHandler<RemoveCharacterAbilityCommand, bool>
    {
        private readonly ICharacterRepository _repository;

        public RemoveCharacterAbilityCommandHandler(ICharacterRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(RemoveCharacterAbilityCommand request, CancellationToken cancellationToken)
        {
            return _repository.RemoveAbilityAsync(request.CharacterId, request.AbilityId, cancellationToken);
        }
    }

    public class AddCharacterHobbyCommandHandler : IRequestHandler<AddCharacterHobbyCommand, bool>
    {
        private readonly ICharacterRepository _repository;
        private readonly IHobbyRepository _hobbyRepository;

        public AddCharacterHobbyCommandHandler(ICharacterRepository repository, IHobbyRepository hobbyRepository)
        {
            _repository = repository;
            _hobbyRepository = hobbyRepository;
        }

        public async Task<bool> Handle(AddCharacterHobbyCommand request, CancellationToken cancellationToken)
        {
            var character = await _repository.FindByIdAsync(request.CharacterId, cancellationToken)
                ?? throw new EntityNotFoundException("Character", request.CharacterId);
            var hobby = await _hobbyRepository.FindByIdAsync(request.HobbyId, cancellationToken)
                ?? throw new DomainValidationException($"Hobby with ID {request.HobbyId} does not exist.");
            if (hobby.WorldId != character.WorldId)
                throw new DomainValidationException($"Hobby with ID {request.HobbyId} does not belong to this world.");
            if (await _repository.IsHobbyLinkedAsync(request.CharacterId, request.HobbyId, cancellationToken))
                throw new DomainValidationException("This hobby is already linked to the character.");

            await _repository.AddHobbyAsync(request.CharacterId, request.HobbyId, cancellationToken);
            return true;
        }
    }

    public class RemoveCharacterHobbyCommandHandler : IRequestHandler<RemoveCharacterHobbyCommand, bool>
    {
        private readonly ICharacterRepository _repository;
        public RemoveCharacterHobbyCommandHandler(ICharacterRepository repository) => _repository = repository;
        public Task<bool> Handle(RemoveCharacterHobbyCommand request, CancellationToken cancellationToken)
            => _repository.RemoveHobbyAsync(request.CharacterId, request.HobbyId, cancellationToken);
    }

    public class AddCharacterSpecialisationCommandHandler : IRequestHandler<AddCharacterSpecialisationCommand, bool>
    {
        private readonly ICharacterRepository _repository;
        private readonly ISpecialisationRepository _specialisationRepository;

        public AddCharacterSpecialisationCommandHandler(ICharacterRepository repository, ISpecialisationRepository specialisationRepository)
        {
            _repository = repository;
            _specialisationRepository = specialisationRepository;
        }

        public async Task<bool> Handle(AddCharacterSpecialisationCommand request, CancellationToken cancellationToken)
        {
            var character = await _repository.FindByIdAsync(request.CharacterId, cancellationToken)
                ?? throw new EntityNotFoundException("Character", request.CharacterId);
            var specialisation = await _specialisationRepository.FindByIdAsync(request.SpecialisationId, cancellationToken)
                ?? throw new DomainValidationException($"Specialisation with ID {request.SpecialisationId} does not exist.");
            if (specialisation.WorldId != character.WorldId)
                throw new DomainValidationException($"Specialisation with ID {request.SpecialisationId} does not belong to this world.");
            if (await _repository.IsSpecialisationLinkedAsync(request.CharacterId, request.SpecialisationId, cancellationToken))
                throw new DomainValidationException("This specialisation is already linked to the character.");

            await _repository.AddSpecialisationAsync(request.CharacterId, request.SpecialisationId, cancellationToken);
            return true;
        }
    }

    public class RemoveCharacterSpecialisationCommandHandler : IRequestHandler<RemoveCharacterSpecialisationCommand, bool>
    {
        private readonly ICharacterRepository _repository;
        public RemoveCharacterSpecialisationCommandHandler(ICharacterRepository repository) => _repository = repository;
        public Task<bool> Handle(RemoveCharacterSpecialisationCommand request, CancellationToken cancellationToken)
            => _repository.RemoveSpecialisationAsync(request.CharacterId, request.SpecialisationId, cancellationToken);
    }

    public class AddCharacterClothingCommandHandler : IRequestHandler<AddCharacterClothingCommand, bool>
    {
        private readonly ICharacterRepository _repository;
        private readonly IClothingRepository _clothingRepository;

        public AddCharacterClothingCommandHandler(ICharacterRepository repository, IClothingRepository clothingRepository)
        {
            _repository = repository;
            _clothingRepository = clothingRepository;
        }

        public async Task<bool> Handle(AddCharacterClothingCommand request, CancellationToken cancellationToken)
        {
            var character = await _repository.FindByIdAsync(request.CharacterId, cancellationToken)
                ?? throw new EntityNotFoundException("Character", request.CharacterId);
            var clothing = await _clothingRepository.FindByIdAsync(request.ClothingId, cancellationToken)
                ?? throw new DomainValidationException($"Clothing with ID {request.ClothingId} does not exist.");
            if (clothing.WorldId != character.WorldId)
                throw new DomainValidationException($"Clothing with ID {request.ClothingId} does not belong to this world.");
            if (await _repository.IsClothingLinkedAsync(request.CharacterId, request.ClothingId, cancellationToken))
                throw new DomainValidationException("This clothing is already linked to the character.");

            await _repository.AddClothingAsync(request.CharacterId, request.ClothingId, cancellationToken);
            return true;
        }
    }

    public class RemoveCharacterClothingCommandHandler : IRequestHandler<RemoveCharacterClothingCommand, bool>
    {
        private readonly ICharacterRepository _repository;
        public RemoveCharacterClothingCommandHandler(ICharacterRepository repository) => _repository = repository;
        public Task<bool> Handle(RemoveCharacterClothingCommand request, CancellationToken cancellationToken)
            => _repository.RemoveClothingAsync(request.CharacterId, request.ClothingId, cancellationToken);
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
            ICharacterRepository repository, IHistoryRepository historyRepository, Character character, CancellationToken cancellationToken)
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

            if (character.HistoryId is int historyId)
            {
                var history = await historyRepository.FindByIdAsync(historyId, cancellationToken)
                    ?? throw new DomainValidationException($"History with ID {historyId} does not exist.");
                if (history.WorldId != character.WorldId)
                {
                    throw new DomainValidationException($"History with ID {historyId} does not belong to this world.");
                }
            }
        }
    }
}
