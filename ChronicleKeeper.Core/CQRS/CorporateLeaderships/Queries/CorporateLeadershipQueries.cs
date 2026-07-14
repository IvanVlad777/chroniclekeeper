using ChronicleKeeper.Core.DTOs.CorporateLeadership;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.CorporateLeaderships.Queries
{
    public class GetAllCorporateLeadershipsQuery : IRequest<List<CorporateLeadershipDto>>
    {
        public int? WorldId { get; set; }
        public int? CorporationId { get; set; }
    }

    public class GetCorporateLeadershipByIdQuery : IRequest<CorporateLeadershipDto?>
    {
        public int Id { get; set; }
    }
}
