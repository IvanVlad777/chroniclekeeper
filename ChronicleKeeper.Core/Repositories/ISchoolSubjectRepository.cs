using ChronicleKeeper.Core.Entities.Social.Education;

namespace ChronicleKeeper.Core.Repositories
{
    public interface ISchoolSubjectRepository
    {
        Task<SchoolSubject> CreateAsync(SchoolSubject subject, CancellationToken cancellationToken = default);
        Task<SchoolSubject?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<SchoolSubject>> GetAllAsync(int? worldId = null, int? schoolId = null, CancellationToken cancellationToken = default);
        Task<SchoolSubject> UpdateAsync(SchoolSubject subject, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
