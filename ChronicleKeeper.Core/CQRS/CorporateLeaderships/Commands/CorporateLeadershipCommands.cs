using ChronicleKeeper.Core.DTOs.CorporateLeadership;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.CorporateLeaderships.Commands
{
    public class CreateCorporateLeadershipCommand : IRequest<CorporateLeadershipDto>
    {
        public CorporateLeadershipCreateDto CorporateLeadershipCreateDto { get; set; } = new();
    }

    public class UpdateCorporateLeadershipCommand : IRequest<CorporateLeadershipDto>
    {
        public int Id { get; set; }
        public CorporateLeadershipUpdateDto CorporateLeadershipUpdateDto { get; set; } = new();
    }

    public class DeleteCorporateLeadershipCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
