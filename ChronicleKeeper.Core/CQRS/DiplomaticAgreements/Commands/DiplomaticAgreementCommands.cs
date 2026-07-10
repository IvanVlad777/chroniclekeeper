using ChronicleKeeper.Core.DTOs.DiplomaticAgreement;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.DiplomaticAgreements.Commands
{
    public class CreateDiplomaticAgreementCommand : IRequest<DiplomaticAgreementDto>
    {
        public DiplomaticAgreementCreateDto DiplomaticAgreementCreateDto { get; set; } = new();
    }

    public class UpdateDiplomaticAgreementCommand : IRequest<DiplomaticAgreementDto>
    {
        public int Id { get; set; }
        public DiplomaticAgreementUpdateDto DiplomaticAgreementUpdateDto { get; set; } = new();
    }

    public class DeleteDiplomaticAgreementCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
