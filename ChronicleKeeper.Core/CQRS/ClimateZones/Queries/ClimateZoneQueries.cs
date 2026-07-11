using ChronicleKeeper.Core.DTOs.ClimateZone;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.ClimateZones.Queries
{
    public class GetAllClimateZonesQuery : IRequest<List<ClimateZoneDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetClimateZoneByIdQuery : IRequest<ClimateZoneDetailsDto?>
    {
        public int Id { get; set; }
    }
}
