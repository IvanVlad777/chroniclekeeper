using ChronicleKeeper.Core.DTOs.World;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Worlds.Commands
{
    public class CreateWorldCommand : IRequest<WorldDto>
    {
        public WorldCreateDto WorldCreateDto { get; set; } = new();
        /// <summary>Identity user ID vlasnika (iz JWT tokena, ne iz bodyja).</summary>
        public string OwnerId { get; set; } = string.Empty;
    }

    public class UpdateWorldCommand : IRequest<WorldDto>
    {
        public int Id { get; set; }
        public WorldUpdateDto WorldUpdateDto { get; set; } = new();
        /// <summary>Tko poziva (iz JWT) — mijenjati smije samo vlasnik ili site admin.</summary>
        public string RequesterId { get; set; } = string.Empty;
        public bool RequesterIsAdmin { get; set; }
    }

    public class DeleteWorldCommand : IRequest<bool>
    {
        public int Id { get; set; }
        /// <summary>Tko poziva (iz JWT) — brisati smije samo vlasnik ili site admin.</summary>
        public string RequesterId { get; set; } = string.Empty;
        public bool RequesterIsAdmin { get; set; }
    }
}
