using ChronicleKeeper.Core.Entities.Geography.Climate;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class WeatherPatternRepository : IWeatherPatternRepository
    {
        private readonly ApplicationDbContext _context;

        public WeatherPatternRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WeatherPattern> CreateAsync(WeatherPattern weatherPattern, CancellationToken cancellationToken = default)
        {
            _context.WeatherPatterns.Add(weatherPattern);
            await _context.SaveChangesAsync(cancellationToken);
            return weatherPattern;
        }

        public async Task<WeatherPattern?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.WeatherPatterns
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
        }

        public async Task<List<WeatherPattern>> GetAllAsync(int? worldId = null, int? climateZoneId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.WeatherPatterns.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(w => w.WorldId == wid);
            }
            if (climateZoneId is int zid)
            {
                query = query.Where(w => w.ClimateZoneId == zid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<WeatherPattern> UpdateAsync(WeatherPattern weatherPattern, CancellationToken cancellationToken = default)
        {
            _context.Entry(weatherPattern).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return weatherPattern;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.WeatherPatterns
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
