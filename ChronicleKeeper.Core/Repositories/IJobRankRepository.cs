using ChronicleKeeper.Core.Entities.Professions;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IJobRankRepository
    {
        Task<JobRank> CreateAsync(JobRank jobRank, CancellationToken cancellationToken = default);
        Task<JobRank?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<JobRank>> GetAllAsync(int? worldId = null, int? professionId = null, CancellationToken cancellationToken = default);
        Task<JobRank> UpdateAsync(JobRank jobRank, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
