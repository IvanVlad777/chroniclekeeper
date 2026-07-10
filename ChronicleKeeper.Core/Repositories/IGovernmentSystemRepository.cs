using ChronicleKeeper.Core.Entities.Social.Politics;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IGovernmentSystemRepository
    {
        Task<GovernmentSystem> CreateAsync(GovernmentSystem governmentSystem, CancellationToken cancellationToken = default);
        /// <summary>With political ideology and parties included — for detail view.</summary>
        Task<GovernmentSystem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<GovernmentSystem?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<GovernmentSystem>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<GovernmentSystem> UpdateAsync(GovernmentSystem governmentSystem, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CountPoliticalPartiesUsingGovernmentSystemAsync(int governmentSystemId, CancellationToken cancellationToken = default);
    }
}
