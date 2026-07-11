using ChronicleKeeper.Core.DTOs.Apprenticeship;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Apprenticeships.Commands
{
    public class CreateApprenticeshipCommand : IRequest<ApprenticeshipDto>
    {
        public ApprenticeshipCreateDto ApprenticeshipCreateDto { get; set; } = new();
    }

    public class UpdateApprenticeshipCommand : IRequest<ApprenticeshipDto>
    {
        public int Id { get; set; }
        public ApprenticeshipUpdateDto ApprenticeshipUpdateDto { get; set; } = new();
    }

    public class DeleteApprenticeshipCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
