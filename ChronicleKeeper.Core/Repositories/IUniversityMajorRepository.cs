using ChronicleKeeper.Core.Entities.Social.Education;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IUniversityMajorRepository
    {
        Task<UniversityMajor> CreateAsync(UniversityMajor major, CancellationToken cancellationToken = default);
        Task<UniversityMajor?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        /// <summary>Full graph (University, Professors, Students) — for detail view.</summary>
        Task<UniversityMajor?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<UniversityMajor>> GetAllAsync(int? worldId = null, int? universityId = null, CancellationToken cancellationToken = default);
        Task<UniversityMajor> UpdateAsync(UniversityMajor major, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> IsProfessorLinkedAsync(int majorId, int characterId, CancellationToken cancellationToken = default);
        Task AddProfessorAsync(int majorId, int characterId, CancellationToken cancellationToken = default);
        Task<bool> RemoveProfessorAsync(int majorId, int characterId, CancellationToken cancellationToken = default);

        Task<bool> IsStudentLinkedAsync(int majorId, int characterId, CancellationToken cancellationToken = default);
        Task AddStudentAsync(int majorId, int characterId, CancellationToken cancellationToken = default);
        Task<bool> RemoveStudentAsync(int majorId, int characterId, CancellationToken cancellationToken = default);
    }
}
