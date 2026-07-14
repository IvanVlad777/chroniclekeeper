using ChronicleKeeper.Core.DTOs.GuildRank;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.GuildRanks.Queries
{
    public class GetAllGuildRanksQuery : IRequest<List<GuildRankDto>>
    {
        public int? WorldId { get; set; }
        public int? GuildId { get; set; }
    }

    public class GetGuildRankByIdQuery : IRequest<GuildRankDto?>
    {
        public int Id { get; set; }
    }
}
