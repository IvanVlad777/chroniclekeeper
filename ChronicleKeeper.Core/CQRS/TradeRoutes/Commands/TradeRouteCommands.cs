using ChronicleKeeper.Core.DTOs.TradeRoute;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.TradeRoutes.Commands
{
    public class CreateTradeRouteCommand : IRequest<TradeRouteDto>
    {
        public TradeRouteCreateDto TradeRouteCreateDto { get; set; } = new();
    }

    public class UpdateTradeRouteCommand : IRequest<TradeRouteDto>
    {
        public int Id { get; set; }
        public TradeRouteUpdateDto TradeRouteUpdateDto { get; set; } = new();
    }

    public class DeleteTradeRouteCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class AddTradeRouteLocationCommand : IRequest<bool>
    {
        public int TradeRouteId { get; set; }
        public int LocationId { get; set; }
    }

    public class RemoveTradeRouteLocationCommand : IRequest<bool>
    {
        public int TradeRouteId { get; set; }
        public int LocationId { get; set; }
    }

    public class AddTradeRouteResourceCommand : IRequest<bool>
    {
        public int TradeRouteId { get; set; }
        public int NaturalResourceId { get; set; }
    }

    public class RemoveTradeRouteResourceCommand : IRequest<bool>
    {
        public int TradeRouteId { get; set; }
        public int NaturalResourceId { get; set; }
    }
}
