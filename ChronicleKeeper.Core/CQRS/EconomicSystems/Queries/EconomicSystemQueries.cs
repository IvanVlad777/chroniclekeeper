using ChronicleKeeper.Core.DTOs.EconomicSystem;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.EconomicSystems.Queries
{
    public class GetAllEconomicSystemsQuery : IRequest<List<EconomicSystemDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetEconomicSystemByIdQuery : IRequest<EconomicSystemDetailsDto?>
    {
        public int Id { get; set; }
    }
}
