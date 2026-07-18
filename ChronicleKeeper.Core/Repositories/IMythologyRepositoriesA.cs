using ChronicleKeeper.Core.Entities.Social.Religions;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IHolySiteRepository
    {
        Task<HolySite> CreateAsync(HolySite entity, CancellationToken ct = default);
        Task<HolySite?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<HolySite?> FindByIdAsync(int id, CancellationToken ct = default);
        Task<List<HolySite>> GetAllAsync(int? worldId = null, CancellationToken ct = default);
        Task<HolySite> UpdateAsync(HolySite entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);

        Task<bool> IsReferencedByFestivalAsync(int holySiteId, CancellationToken ct = default);
    }

    public interface IReligiousTextRepository
    {
        Task<ReligiousText> CreateAsync(ReligiousText entity, CancellationToken ct = default);
        Task<ReligiousText?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ReligiousText?> FindByIdAsync(int id, CancellationToken ct = default);
        Task<List<ReligiousText>> GetAllAsync(int? worldId = null, CancellationToken ct = default);
        Task<ReligiousText> UpdateAsync(ReligiousText entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }

    public interface IReligiousFestivalRepository
    {
        Task<ReligiousFestival> CreateAsync(ReligiousFestival entity, CancellationToken ct = default);
        Task<ReligiousFestival?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<ReligiousFestival?> FindByIdAsync(int id, CancellationToken ct = default);
        Task<List<ReligiousFestival>> GetAllAsync(int? worldId = null, CancellationToken ct = default);
        Task<ReligiousFestival> UpdateAsync(ReligiousFestival entity, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
