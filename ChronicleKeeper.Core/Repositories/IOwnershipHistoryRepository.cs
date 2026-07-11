using ChronicleKeeper.Core.Entities.Characters.Equipment;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IOwnershipHistoryRepository
    {
        Task<OwnershipHistory> CreateAsync(OwnershipHistory ownershipHistory, CancellationToken cancellationToken = default);
        Task<OwnershipHistory?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<OwnershipHistory>> GetAllAsync(int? worldId = null, int? itemId = null, CancellationToken cancellationToken = default);
        Task<OwnershipHistory> UpdateAsync(OwnershipHistory ownershipHistory, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
