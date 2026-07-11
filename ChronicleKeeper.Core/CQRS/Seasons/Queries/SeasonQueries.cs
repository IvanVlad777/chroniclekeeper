using ChronicleKeeper.Core.DTOs.Season;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Seasons.Queries
{
    public class GetAllSeasonsQuery : IRequest<List<SeasonDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetSeasonByIdQuery : IRequest<SeasonDto?>
    {
        public int Id { get; set; }
    }
}
