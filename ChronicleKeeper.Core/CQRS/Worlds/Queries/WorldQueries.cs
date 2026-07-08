using ChronicleKeeper.Core.DTOs.World;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Worlds.Queries
{
    public class GetAllWorldsQuery : IRequest<List<WorldDto>>
    {
        /// <summary>Ako je postavljeno, vraća samo svjetove tog vlasnika.</summary>
        public string? OwnerId { get; set; }
    }

    public class GetWorldByIdQuery : IRequest<WorldDto?>
    {
        public int Id { get; set; }
    }
}
