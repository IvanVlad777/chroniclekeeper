using ChronicleKeeper.Core.DTOs.Corporation;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Corporations.Queries
{
    public class GetAllCorporationsQuery : IRequest<List<CorporationDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetCorporationByIdQuery : IRequest<CorporationDetailsDto?>
    {
        public int Id { get; set; }
    }
}
