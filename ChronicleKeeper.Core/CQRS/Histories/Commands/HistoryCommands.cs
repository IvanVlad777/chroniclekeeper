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
}
