using ChronicleKeeper.Core.DTOs.OwnershipHistory;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.OwnershipHistories.Commands
{
    public class CreateOwnershipHistoryCommand : IRequest<OwnershipHistoryDto>
    {
        public OwnershipHistoryCreateDto OwnershipHistoryCreateDto { get; set; } = new();
    }

    public class UpdateOwnershipHistoryCommand : IRequest<OwnershipHistoryDto>
    {
        public int Id { get; set; }
        public OwnershipHistoryUpdateDto OwnershipHistoryUpdateDto { get; set; } = new();
    }

    public class DeleteOwnershipHistoryCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
