using ChronicleKeeper.Core.Entities.Social.Economy;

namespace ChronicleKeeper.Core.Repositories
{
    public interface ITradeRouteRepository
    {
        Task<TradeRoute> CreateAsync(TradeRoute tradeRoute, CancellationToken cancellationToken = default);
        /// <summary>S uključenim History-jem i cross-link kolekcijama — za detail prikaz.</summary>
        Task<TradeRoute?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<TradeRoute?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<TradeRoute>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<TradeRoute> UpdateAsync(TradeRoute tradeRoute, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> IsLocationLinkedAsync(int tradeRouteId, int locationId, CancellationToken cancellationToken = default);
        Task AddLocationAsync(int tradeRouteId, int locationId, CancellationToken cancellationToken = default);
        Task<bool> RemoveLocationAsync(int tradeRouteId, int locationId, CancellationToken cancellationToken = default);

        Task<bool> IsResourceLinkedAsync(int tradeRouteId, int naturalResourceId, CancellationToken cancellationToken = default);
        Task AddResourceAsync(int tradeRouteId, int naturalResourceId, CancellationToken cancellationToken = default);
        Task<bool> RemoveResourceAsync(int tradeRouteId, int naturalResourceId, CancellationToken cancellationToken = default);
    }
}
