using ChronicleKeeper.Core.Entities.Social.Education;

namespace ChronicleKeeper.Core.Repositories
{
    /// <summary>
    /// Upravlja bazom Schools tablice (TPH). Upiti kroz DbSet&lt;School&gt; vraćaju
    /// i School i TradeSchool retke — namjerno, za generički prikaz svih škola.
    /// </summary>
    public interface ISchoolRepository
    {
        Task<School> CreateAsync(School school, CancellationToken cancellationToken = default);
        /// <summary>S uključenim predmetima i alumnima — za detail prikaz.</summary>
        Task<School?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<School?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<School>> GetAllAsync(int? worldId = null, int? educationSystemId = null, CancellationToken cancellationToken = default);
        Task<School> UpdateAsync(School school, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CountEducationRecordsUsingSchoolAsync(int schoolId, CancellationToken cancellationToken = default);
    }
}
