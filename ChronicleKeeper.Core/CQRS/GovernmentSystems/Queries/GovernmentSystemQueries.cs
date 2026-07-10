using ChronicleKeeper.Core.DTOs.GovernmentSystem;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.GovernmentSystems.Queries
{
    public class GetAllGovernmentSystemsQuery : IRequest<List<GovernmentSystemDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetGovernmentSystemByIdQuery : IRequest<GovernmentSystemDetailsDto?>
    {
        public int Id { get; set; }
    }
}
