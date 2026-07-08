using ChronicleKeeper.Core.Entities.Worlds;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IWorldRepository
    {
        Task<World> CreateAsync(World world, CancellationToken cancellationToken = default);
        Task<World?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<World>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<World>> GetByOwnerAsync(string ownerId, CancellationToken cancellationToken = default);
        Task<World> UpdateAsync(World world, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    }
}
