using ChronicleKeeper.Core.Entities.Social.Cultures;

namespace ChronicleKeeper.Core.Repositories
{
    public interface ICultureRepository
    {
        Task<Culture> CreateAsync(Culture culture, CancellationToken cancellationToken = default);
        /// <summary>S uključenim jezikom i religijom — za detail prikaz.</summary>
        Task<Culture?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Culture?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Culture>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<Culture> UpdateAsync(Culture culture, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
