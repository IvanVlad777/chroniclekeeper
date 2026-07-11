using ChronicleKeeper.Core.Entities.Social.Education;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IEducationRecordRepository
    {
        Task<EducationRecord> CreateAsync(EducationRecord record, CancellationToken cancellationToken = default);
        Task<EducationRecord?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<EducationRecord>> GetAllAsync(int? worldId = null, int? characterId = null, int? schoolId = null, int? universityId = null, CancellationToken cancellationToken = default);
        Task<EducationRecord> UpdateAsync(EducationRecord record, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
