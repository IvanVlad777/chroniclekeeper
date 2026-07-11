using ChronicleKeeper.Core.DTOs.OwnershipHistory;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.OwnershipHistories.Queries
{
    public class GetAllOwnershipHistoriesQuery : IRequest<List<OwnershipHistoryDto>>
    {
        public int? WorldId { get; set; }
        public int? ItemId { get; set; }
    }

    public class GetOwnershipHistoryByIdQuery : IRequest<OwnershipHistoryDto?>
    {
        public int Id { get; set; }
    }
}
