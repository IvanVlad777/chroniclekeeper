using ChronicleKeeper.Core.Entities.Social.Politics;

namespace ChronicleKeeper.Core.Repositories
{
    public interface ILegalSystemRepository
    {
        Task<LegalSystem> CreateAsync(LegalSystem legalSystem, CancellationToken cancellationToken = default);
        Task<LegalSystem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<LegalSystem?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<LegalSystem>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<LegalSystem> UpdateAsync(LegalSystem legalSystem, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
