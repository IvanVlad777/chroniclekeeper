using ChronicleKeeper.Core.DTOs.TradeRoute;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.TradeRoutes.Queries
{
    public class GetAllTradeRoutesQuery : IRequest<List<TradeRouteDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetTradeRouteByIdQuery : IRequest<TradeRouteDetailsDto?>
    {
        public int Id { get; set; }
    }
}
