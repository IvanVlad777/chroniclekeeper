using ChronicleKeeper.Core.DTOs.DiplomaticAgreement;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.DiplomaticAgreements.Queries
{
    public class GetAllDiplomaticAgreementsQuery : IRequest<List<DiplomaticAgreementDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetDiplomaticAgreementByIdQuery : IRequest<DiplomaticAgreementDetailsDto?>
    {
        public int Id { get; set; }
    }
}
