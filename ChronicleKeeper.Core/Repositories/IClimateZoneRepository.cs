using ChronicleKeeper.Core.Entities.Geography.Climate;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IClimateZoneRepository
    {
        Task<ClimateZone> CreateAsync(ClimateZone climateZone, CancellationToken cancellationToken = default);
        /// <summary>S uključenim History-jem, weather patternima i cross-link kolekcijama — za detail prikaz.</summary>
        Task<ClimateZone?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ClimateZone?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<ClimateZone>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default);
        Task<ClimateZone> UpdateAsync(ClimateZone climateZone, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> IsClimateDetailLinkedAsync(int climateZoneId, int climateDetailId, CancellationToken cancellationToken = default);
        Task AddClimateDetailAsync(int climateZoneId, int climateDetailId, CancellationToken cancellationToken = default);
        Task<bool> RemoveClimateDetailAsync(int climateZoneId, int climateDetailId, CancellationToken cancellationToken = default);

        Task<bool> IsSeasonLinkedAsync(int climateZoneId, int seasonId, CancellationToken cancellationToken = default);
        Task AddSeasonAsync(int climateZoneId, int seasonId, CancellationToken cancellationToken = default);
        Task<bool> RemoveSeasonAsync(int climateZoneId, int seasonId, CancellationToken cancellationToken = default);

        Task<bool> IsLocationLinkedAsync(int climateZoneId, int locationId, CancellationToken cancellationToken = default);
        Task AddLocationAsync(int climateZoneId, int locationId, CancellationToken cancellationToken = default);
        Task<bool> RemoveLocationAsync(int climateZoneId, int locationId, CancellationToken cancellationToken = default);
    }
}
