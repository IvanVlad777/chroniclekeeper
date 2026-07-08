using ChronicleKeeper.Core.DTOs.Location;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Locations.Queries
{
    public class GetAllLocationsQuery : IRequest<List<LocationDto>>
    {
        /// <summary>Ako je postavljeno, vraća samo lokacije tog svijeta.</summary>
        public int? WorldId { get; set; }
    }

    public class GetLocationByIdQuery : IRequest<LocationDetailsDto?>
    {
        public int Id { get; set; }
    }
}
