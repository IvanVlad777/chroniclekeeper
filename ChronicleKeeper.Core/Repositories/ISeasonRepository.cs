using ChronicleKeeper.Core.Entities.Geography.Climate;

namespace ChronicleKeeper.Core.Repositories
{
    public interface ISeasonRepository
    {
        Task<Season> CreateAsync(Season season, CancellationToken cancellationToken = default);
        Task<Season?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Season>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<Season> UpdateAsync(Season season, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
