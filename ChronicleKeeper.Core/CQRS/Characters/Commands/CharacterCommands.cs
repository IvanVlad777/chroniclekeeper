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
}
