using ChronicleKeeper.Core.DTOs.Character;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Characters.Queries
{
    public class GetAllCharactersQuery : IRequest<List<CharacterDto>>
    {
        // Možemo dodati pagination, filtering parametre kasnije
    }

    public class GetCharacterByIdQuery : IRequest<CharacterDto?>
    {
        public int Id { get; set; }
    }
}
