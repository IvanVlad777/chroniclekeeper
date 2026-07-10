using ChronicleKeeper.Core.DTOs.GovernmentSystem;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.GovernmentSystems.Commands
{
    public class CreateGovernmentSystemCommand : IRequest<GovernmentSystemDto>
    {
        public GovernmentSystemCreateDto GovernmentSystemCreateDto { get; set; } = new();
    }

    public class UpdateGovernmentSystemCommand : IRequest<GovernmentSystemDto>
    {
        public int Id { get; set; }
        public GovernmentSystemUpdateDto GovernmentSystemUpdateDto { get; set; } = new();
    }

    public class DeleteGovernmentSystemCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
