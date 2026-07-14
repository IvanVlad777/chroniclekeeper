using ChronicleKeeper.Core.DTOs.TaxationSystem;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.TaxationSystems.Queries
{
    public class GetAllTaxationSystemsQuery : IRequest<List<TaxationSystemDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetTaxationSystemByIdQuery : IRequest<TaxationSystemDetailsDto?>
    {
        public int Id { get; set; }
    }
}
