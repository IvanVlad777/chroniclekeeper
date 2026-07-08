using ChronicleKeeper.Core.DTOs.Character;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Characters.Queries
{
    public class GetAllCharactersQuery : IRequest<List<CharacterDto>>
    {
        /// <summary>Ako je postavljeno, vraća samo likove tog svijeta.</summary>
        public int? WorldId { get; set; }
        // Možemo dodati pagination kasnije
    }

    public class GetCharacterByIdQuery : IRequest<CharacterDto?>
    {
        public int Id { get; set; }
    }
}
