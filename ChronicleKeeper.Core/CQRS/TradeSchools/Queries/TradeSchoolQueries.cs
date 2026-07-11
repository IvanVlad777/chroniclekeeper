using ChronicleKeeper.Core.DTOs.TradeSchool;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.TradeSchools.Queries
{
    public class GetAllTradeSchoolsQuery : IRequest<List<TradeSchoolDto>>
    {
        public int? WorldId { get; set; }
        public int? EducationSystemId { get; set; }
    }

    public class GetTradeSchoolByIdQuery : IRequest<TradeSchoolDetailsDto?>
    {
        public int Id { get; set; }
    }
}
