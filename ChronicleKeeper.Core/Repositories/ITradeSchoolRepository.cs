using ChronicleKeeper.Core.Entities.Professions;

namespace ChronicleKeeper.Core.Repositories
{
    /// <summary>
    /// Upravlja TradeSchool retcima u zajedničkoj Schools tablici (TPH).
    /// DbSet&lt;TradeSchool&gt; EF Core automatski filtrira na SchoolType = 'TradeSchool'.
    /// </summary>
    public interface ITradeSchoolRepository
    {
        Task<TradeSchool> CreateAsync(TradeSchool tradeSchool, CancellationToken cancellationToken = default);
        /// <summary>S uključenim predmetima, alumnima, obučenim profesijama i naukovanjima — za detail prikaz.</summary>
        Task<TradeSchool?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<TradeSchool?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<TradeSchool>> GetAllAsync(int? worldId = null, int? educationSystemId = null, CancellationToken cancellationToken = default);
        Task<TradeSchool> UpdateAsync(TradeSchool tradeSchool, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CountApprenticeshipsUsingTradeSchoolAsync(int tradeSchoolId, CancellationToken cancellationToken = default);
        /// <summary>TradeSchool retci dijele Schools tablicu, pa EducationRecord.SchoolId može pokazivati na njih.</summary>
        Task<int> CountEducationRecordsUsingSchoolAsync(int schoolId, CancellationToken cancellationToken = default);
    }
}
