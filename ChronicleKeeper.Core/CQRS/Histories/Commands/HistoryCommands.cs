using ChronicleKeeper.Core.DTOs.History;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Histories.Commands
{
    public class CreateHistoryCommand : IRequest<HistoryDto>
    {
        public HistoryCreateDto HistoryCreateDto { get; set; } = new();
    }

    public class UpdateHistoryCommand : IRequest<HistoryDto>
    {
        public int Id { get; set; }
        public HistoryUpdateDto HistoryUpdateDto { get; set; } = new();
    }

    public class DeleteHistoryCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    /// <summary>Attach this history to a target entity (sets the target's HistoryId).</summary>
    public class LinkHistoryCommand : IRequest<Unit>
    {
        public int HistoryId { get; set; }
        public HistoryLinkTargetType TargetType { get; set; }
        public int TargetId { get; set; }
    }

    /// <summary>Detach this history from a target entity (clears the target's HistoryId).</summary>
    public class UnlinkHistoryCommand : IRequest<bool>
    {
        public int HistoryId { get; set; }
        public HistoryLinkTargetType TargetType { get; set; }
        public int TargetId { get; set; }
    }
}
