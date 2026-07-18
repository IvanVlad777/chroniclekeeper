using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Entities.Social.Religions;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IReligiousOrderRepository
    {
        Task<ReligiousOrder> CreateAsync(ReligiousOrder entity, CancellationToken ct = default);
        Task<ReligiousOrder?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ReligiousOrder?> FindByIdAsync(int id, CancellationToken ct = default);
        Task<List<ReligiousOrder>> GetAllAsync(int? worldId = null, CancellationToken ct = default);
        Task<ReligiousOrder> UpdateAsync(ReligiousOrder entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);

        Task<bool> FactionExistsInWorldAsync(int factionId, int worldId, CancellationToken ct = default);
        Task AddFactionAsync(int orderId, int factionId, CancellationToken ct = default);
        Task RemoveFactionAsync(int orderId, int factionId, CancellationToken ct = default);
    }

    public interface IDeityRepository
    {
        Task<Deity> CreateAsync(Deity entity, CancellationToken ct = default);
        Task<Deity?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Deity?> FindByIdAsync(int id, CancellationToken ct = default);
        Task<List<Deity>> GetAllAsync(int? worldId = null, CancellationToken ct = default);
        Task<Deity> UpdateAsync(Deity entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);

        // ReligiousOrder M:N (DeityReligiousOrder) — owner side.
        Task<bool> OrderExistsInWorldAsync(int orderId, int worldId, CancellationToken ct = default);
        Task AddOrderAsync(int deityId, int orderId, CancellationToken ct = default);
        Task RemoveOrderAsync(int deityId, int orderId, CancellationToken ct = default);

        // Self-referencing M:N (DeityAlliance / DeityRivalry) — owner side.
        Task<bool> DeityExistsInWorldAsync(int deityId, int worldId, CancellationToken ct = default);
        Task<bool> AllianceExistsAsync(int deityId, int alliedDeityId, CancellationToken ct = default);
        Task AddAllyAsync(int deityId, int alliedDeityId, CancellationToken ct = default);
        Task RemoveAllyAsync(int deityId, int alliedDeityId, CancellationToken ct = default);
        Task<bool> RivalryExistsAsync(int deityId, int rivalDeityId, CancellationToken ct = default);
        Task AddRivalAsync(int deityId, int rivalDeityId, CancellationToken ct = default);
        Task RemoveRivalAsync(int deityId, int rivalDeityId, CancellationToken ct = default);
    }
}
