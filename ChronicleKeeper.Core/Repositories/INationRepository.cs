using ChronicleKeeper.Core.Entities.Social.Nationality;

namespace ChronicleKeeper.Core.Repositories
{
    public interface INationRepository
    {
        Task<Nation> CreateAsync(Nation nation, CancellationToken cancellationToken = default);
        /// <summary>S uključenim građanima — za detail prikaz.</summary>
        Task<Nation?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Nation?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Nation>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<Nation> UpdateAsync(Nation nation, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CountCharactersUsingNationAsync(int nationId, CancellationToken cancellationToken = default);
    }
}
