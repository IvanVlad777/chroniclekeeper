using ChronicleKeeper.Core.DTOs.ClimateDetail;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.ClimateDetails.Queries
{
    public class GetAllClimateDetailsQuery : IRequest<List<ClimateDetailDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetClimateDetailByIdQuery : IRequest<ClimateDetailDto?>
    {
        public int Id { get; set; }
    }
}
