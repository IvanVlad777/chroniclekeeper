using ChronicleKeeper.Core.DTOs.Guild;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.Guilds.Queries
{
    public class GetAllGuildsQuery : IRequest<List<GuildDto>>
    {
        public int? WorldId { get; set; }
    }

    public class GetGuildByIdQuery : IRequest<GuildDetailsDto?>
    {
        public int Id { get; set; }
    }
}
