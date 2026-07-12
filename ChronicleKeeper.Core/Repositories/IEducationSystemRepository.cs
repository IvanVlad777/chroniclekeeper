using ChronicleKeeper.Core.Entities.Social.Education;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IEducationSystemRepository
    {
        Task<EducationSystem> CreateAsync(EducationSystem educationSystem, CancellationToken cancellationToken = default);
        /// <summary>S uključenim školama i sveučilištima — za detail prikaz.</summary>
        Task<EducationSystem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<EducationSystem?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<EducationSystem>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<EducationSystem> UpdateAsync(EducationSystem educationSystem, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CountLocationsUsingEducationSystemAsync(int educationSystemId, CancellationToken cancellationToken = default);
    }
}
