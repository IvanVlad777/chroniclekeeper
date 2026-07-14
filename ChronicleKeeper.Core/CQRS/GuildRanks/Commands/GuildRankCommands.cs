using ChronicleKeeper.Core.DTOs.GuildRank;
using MediatR;

namespace ChronicleKeeper.Core.CQRS.GuildRanks.Commands
{
    public class CreateGuildRankCommand : IRequest<GuildRankDto>
    {
        public GuildRankCreateDto GuildRankCreateDto { get; set; } = new();
    }

    public class UpdateGuildRankCommand : IRequest<GuildRankDto>
    {
        public int Id { get; set; }
        public GuildRankUpdateDto GuildRankUpdateDto { get; set; } = new();
    }

    public class DeleteGuildRankCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
