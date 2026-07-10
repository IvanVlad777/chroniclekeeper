using ChronicleKeeper.Core.DTOs.PoliticalParty;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.PoliticalParties.Queries
{
    public class GetAllPoliticalPartiesQuery : IRequest<List<PoliticalPartyDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetPoliticalPartyByIdQuery : IRequest<PoliticalPartyDetailsDto?>
    {
        public int Id { get; set; }
    }
}
