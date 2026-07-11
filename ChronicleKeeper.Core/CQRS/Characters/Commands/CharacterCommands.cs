using ChronicleKeeper.Core.DTOs.Character;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Characters.Commands
{
    public class CreateCharacterCommand : IRequest<CharacterDto>
    {
        public CharacterCreateDto CharacterCreateDto { get; set; } = new();
    }

    public class UpdateCharacterCommand : IRequest<CharacterDto>
    {
        public int Id { get; set; }
        public CharacterUpdateDto CharacterUpdateDto { get; set; } = new();
    }

    public class DeleteCharacterCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class AddCharacterRelationshipCommand : IRequest<CharacterRelationshipDto>
    {
        public int CharacterId { get; set; }
        public CharacterRelationshipCreateDto RelationshipDto { get; set; } = new();
    }

    public class RemoveCharacterRelationshipCommand : IRequest<bool>
    {
        public int CharacterId { get; set; }
        public int RelationshipId { get; set; }
    }

    public class AddCharacterAbilityCommand : IRequest<bool>
    {
        public int CharacterId { get; set; }
        public int AbilityId { get; set; }
    }

    public class RemoveCharacterAbilityCommand : IRequest<bool>
    {
        public int CharacterId { get; set; }
        public int AbilityId { get; set; }
    }
}
