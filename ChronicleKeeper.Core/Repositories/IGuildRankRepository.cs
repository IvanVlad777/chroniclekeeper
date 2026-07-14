using ChronicleKeeper.Core.Entities.Social.Economy;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IGuildRankRepository
    {
        Task<GuildRank> CreateAsync(GuildRank guildRank, CancellationToken cancellationToken = default);
        Task<List<GuildRank>> GetAllAsync(int? worldId = null, int? guildId = null, CancellationToken cancellationToken = default);
        Task<GuildRank?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<GuildRank> UpdateAsync(GuildRank guildRank, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
