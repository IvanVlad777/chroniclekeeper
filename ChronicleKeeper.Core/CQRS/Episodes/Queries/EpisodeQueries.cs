using ChronicleKeeper.Core.DTOs.Episode;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Episodes.Queries
{
    public class GetAllEpisodesQuery : IRequest<List<EpisodeDto>>
    {
        public int? WorldId { get; set; }
        public int? SeriesId { get; set; }
    }

    public class GetEpisodeByIdQuery : IRequest<EpisodeDto?>
    {
        public int Id { get; set; }
    }
}
