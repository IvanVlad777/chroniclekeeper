using ChronicleKeeper.Core.Entities.Geography.Climate;

namespace ChronicleKeeper.Core.Repositories
{
    public interface IWeatherPatternRepository
    {
        Task<WeatherPattern> CreateAsync(WeatherPattern weatherPattern, CancellationToken cancellationToken = default);
        Task<WeatherPattern?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<WeatherPattern>> GetAllAsync(int? worldId = null, int? climateZoneId = null, CancellationToken cancellationToken = default);
        Task<WeatherPattern> UpdateAsync(WeatherPattern weatherPattern, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
