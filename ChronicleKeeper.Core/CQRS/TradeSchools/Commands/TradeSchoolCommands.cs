using ChronicleKeeper.Core.DTOs.TradeSchool;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.TradeSchools.Commands
{
    public class CreateTradeSchoolCommand : IRequest<TradeSchoolDto>
    {
        public TradeSchoolCreateDto TradeSchoolCreateDto { get; set; } = new();
    }

    public class UpdateTradeSchoolCommand : IRequest<TradeSchoolDto>
    {
        public int Id { get; set; }
        public TradeSchoolUpdateDto TradeSchoolUpdateDto { get; set; } = new();
    }

    public class DeleteTradeSchoolCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
