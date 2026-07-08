using ChronicleKeeper.Core.DTOs.Faction;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Factions.Commands
{
    public class CreateFactionCommand : IRequest<FactionDto>
    {
        public FactionCreateDto FactionCreateDto { get; set; } = new();
    }

    public class UpdateFactionCommand : IRequest<FactionDto>
    {
        public int Id { get; set; }
        public FactionUpdateDto FactionUpdateDto { get; set; } = new();
    }

    public class DeleteFactionCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class AddFactionMemberCommand : IRequest<FactionMemberDto>
    {
        public int FactionId { get; set; }
        public FactionMemberAddDto MemberDto { get; set; } = new();
    }

    public class RemoveFactionMemberCommand : IRequest<bool>
    {
        public int FactionId { get; set; }
        public int CharacterId { get; set; }
    }
}
