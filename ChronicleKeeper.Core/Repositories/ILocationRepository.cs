using ChronicleKeeper.Core.Entities.Geography;

namespace ChronicleKeeper.Core.Repositories
{
    public interface ILocationRepository
    {
        Task<Location> CreateAsync(Location location, CancellationToken cancellationToken = default);
        /// <summary>Puni graf (parent, djeca, tagovi) — za detail prikaz.</summary>
        Task<Location?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        /// <summary>Samo korijenski entitet, bez Include-ova.</summary>
        Task<Location?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Location>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<Location> UpdateAsync(Location location, CancellationToken cancellationToken = default);
        /// <summary>Vraća false ako lokacija ne postoji.</summary>
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistsInWorldAsync(int locationId, int worldId, CancellationToken cancellationToken = default);
        Task<bool> HasChildrenAsync(int locationId, CancellationToken cancellationToken = default);
        /// <summary>Bi li postavljanje newParentId kao roditelja lokacije stvorilo ciklus u hijerarhiji.</summary>
        Task<bool> WouldCreateCycleAsync(int locationId, int newParentId, CancellationToken cancellationToken = default);
        /// <summary>Je li lokacija postavljena kao SourceLocation/MouthLocation neke rijeke (RiverEcosystem.Restrict FK-ovi).</summary>
        Task<bool> IsReferencedAsRiverEndpointAsync(int locationId, CancellationToken cancellationToken = default);

        Task<bool> IsReferencedByHolySiteAsync(int locationId, CancellationToken cancellationToken = default);

        Task<bool> IsNativeSpeciesLinkedAsync(int regionId, int sapientSpeciesId, CancellationToken cancellationToken = default);
        Task AddNativeSpeciesAsync(int regionId, int sapientSpeciesId, CancellationToken cancellationToken = default);
        Task<bool> RemoveNativeSpeciesAsync(int regionId, int sapientSpeciesId, CancellationToken cancellationToken = default);
    }
}
