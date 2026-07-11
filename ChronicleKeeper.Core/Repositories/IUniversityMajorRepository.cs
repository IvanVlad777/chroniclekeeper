using ChronicleKeeper.Core.Entities.Social.Education;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IUniversityMajorRepository
    {
        Task<UniversityMajor> CreateAsync(UniversityMajor major, CancellationToken cancellationToken = default);
        Task<UniversityMajor?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<UniversityMajor>> GetAllAsync(int? worldId = null, int? universityId = null, CancellationToken cancellationToken = default);
        Task<UniversityMajor> UpdateAsync(UniversityMajor major, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
