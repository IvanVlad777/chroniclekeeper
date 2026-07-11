using ChronicleKeeper.Core.Entities.Geography.Climate;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IClimateDetailRepository
    {
        Task<ClimateDetail> CreateAsync(ClimateDetail climateDetail, CancellationToken cancellationToken = default);
        Task<ClimateDetail?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<ClimateDetail>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<ClimateDetail> UpdateAsync(ClimateDetail climateDetail, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
