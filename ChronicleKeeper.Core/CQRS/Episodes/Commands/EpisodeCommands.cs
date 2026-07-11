using ChronicleKeeper.Core.DTOs.Episode;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Episodes.Commands
{
    public class CreateEpisodeCommand : IRequest<EpisodeDto>
    {
        public EpisodeCreateDto EpisodeCreateDto { get; set; } = new();
    }

    public class UpdateEpisodeCommand : IRequest<EpisodeDto>
    {
        public int Id { get; set; }
        public EpisodeUpdateDto EpisodeUpdateDto { get; set; } = new();
    }

    public class DeleteEpisodeCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
