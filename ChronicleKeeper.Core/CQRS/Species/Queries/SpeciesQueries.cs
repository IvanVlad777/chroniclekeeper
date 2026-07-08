using ChronicleKeeper.Core.DTOs.Species;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Species.Queries
{
    public class GetAllSpeciesQuery : IRequest<List<SpeciesDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetSpeciesByIdQuery : IRequest<SpeciesDetailsDto?>
    {
        public int Id { get; set; }
    }

    public class GetAllRacesQuery : IRequest<List<RaceDto>>
    {
        public int? WorldId { get; set; }
        public int? SpeciesId { get; set; }
    }

    public class GetRaceByIdQuery : IRequest<RaceDto?>
    {
        public int Id { get; set; }
    }
}
