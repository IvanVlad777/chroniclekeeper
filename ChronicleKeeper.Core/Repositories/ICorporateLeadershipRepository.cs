using ChronicleKeeper.Core.Entities.Social.Economy;

namespace ChronicleKeeper.Core.Repositories
{
    public interface ICorporateLeadershipRepository
    {
        Task<CorporateLeadership> CreateAsync(CorporateLeadership leadership, CancellationToken cancellationToken = default);
        Task<List<CorporateLeadership>> GetAllAsync(int? worldId = null, int? corporationId = null, CancellationToken cancellationToken = default);
        Task<CorporateLeadership?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<CorporateLeadership> UpdateAsync(CorporateLeadership leadership, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
