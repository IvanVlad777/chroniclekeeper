using ChronicleKeeper.Core.Entities.Social.Economy;

namespace ChronicleKeeper.Core.Repositories
{
    public interface INaturalResourceRepository
    {
        Task<NaturalResource> CreateAsync(NaturalResource naturalResource, CancellationToken cancellationToken = default);
        /// <summary>S uključenim ExtractionMethod/History i cross-link kolekcijama — za detail prikaz.</summary>
        Task<NaturalResource?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<NaturalResource?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<NaturalResource>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<NaturalResource> UpdateAsync(NaturalResource naturalResource, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> IsLocationLinkedAsync(int naturalResourceId, int locationId, CancellationToken cancellationToken = default);
        Task AddLocationAsync(int naturalResourceId, int locationId, CancellationToken cancellationToken = default);
        Task<bool> RemoveLocationAsync(int naturalResourceId, int locationId, CancellationToken cancellationToken = default);
    }
}
