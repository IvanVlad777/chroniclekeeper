using ChronicleKeeper.Core.DTOs.History;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Histories.Queries
{
    public class GetAllHistoriesQuery : IRequest<List<HistoryDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetHistoryByIdQuery : IRequest<HistoryDetailsDto?>
    {
        public int Id { get; set; }
    }
}
