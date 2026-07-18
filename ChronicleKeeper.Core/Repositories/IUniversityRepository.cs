using ChronicleKeeper.Core.Entities.Social.Education;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IUniversityRepository
    {
        Task<University> CreateAsync(University university, CancellationToken cancellationToken = default);
        /// <summary>S uključenim smjerovima i alumnima — za detail prikaz.</summary>
        Task<University?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        /// <summary>Samo korijenski entitet, bez Include-ova — za update/interne provjere.</summary>
        Task<University?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<University>> GetAllAsync(int? worldId = null, int? educationSystemId = null, CancellationToken cancellationToken = default);
        Task<University> UpdateAsync(University university, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ExistsInWorldAsync(int universityId, int worldId, CancellationToken cancellationToken = default);
        Task<int> CountEducationRecordsUsingUniversityAsync(int universityId, CancellationToken cancellationToken = default);
        Task<int> CountLibrariesUsingUniversityAsync(int universityId, CancellationToken cancellationToken = default);

        // Students / Professors (UniversityStudent / UniversityProfessor joins with Character)
        Task<bool> IsStudentLinkedAsync(int universityId, int characterId, CancellationToken cancellationToken = default);
        Task AddStudentAsync(int universityId, int characterId, CancellationToken cancellationToken = default);
        Task<bool> RemoveStudentAsync(int universityId, int characterId, CancellationToken cancellationToken = default);
        Task<bool> IsProfessorLinkedAsync(int universityId, int characterId, CancellationToken cancellationToken = default);
        Task AddProfessorAsync(int universityId, int characterId, CancellationToken cancellationToken = default);
        Task<bool> RemoveProfessorAsync(int universityId, int characterId, CancellationToken cancellationToken = default);
    }
}
