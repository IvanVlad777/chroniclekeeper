using ChronicleKeeper.Core.DTOs.Faction;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Factions.Queries
{
    public class GetAllFactionsQuery : IRequest<List<FactionDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetFactionByIdQuery : IRequest<FactionDetailsDto?>
    {
        public int Id { get; set; }
    }
}
