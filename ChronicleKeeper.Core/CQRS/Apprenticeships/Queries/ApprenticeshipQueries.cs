using ChronicleKeeper.Core.DTOs.Apprenticeship;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Apprenticeships.Queries
{
    public class GetAllApprenticeshipsQuery : IRequest<List<ApprenticeshipDto>>
    {
        public int? WorldId { get; set; }
        public int? ProfessionId { get; set; }
    }

    public class GetApprenticeshipByIdQuery : IRequest<ApprenticeshipDto?>
    {
        public int Id { get; set; }
    }
}
