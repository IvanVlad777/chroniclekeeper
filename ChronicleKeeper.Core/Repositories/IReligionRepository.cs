using ChronicleKeeper.Core.Entities.Social.Religions;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IReligionRepository
    {
        Task<Religion> CreateAsync(Religion religion, CancellationToken cancellationToken = default);
        /// <summary>S uključenim sljedbenicima — za detail prikaz.</summary>
        Task<Religion?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Religion?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Religion>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<Religion> UpdateAsync(Religion religion, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CountCharactersUsingReligionAsync(int religionId, CancellationToken cancellationToken = default);
        Task<int> CountCulturesUsingReligionAsync(int religionId, CancellationToken cancellationToken = default);
    }
}
