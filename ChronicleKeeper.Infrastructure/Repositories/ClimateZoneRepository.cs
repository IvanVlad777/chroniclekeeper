using ChronicleKeeper.Core.Entities.Geography.Climate;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class ClimateZoneRepository : IClimateZoneRepository
    {
        private readonly ApplicationDbContext _context;

        public ClimateZoneRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ClimateZone> CreateAsync(ClimateZone climateZone, CancellationToken cancellationToken = default)
        {
            _context.ClimateZones.Add(climateZone);
            await _context.SaveChangesAsync(cancellationToken);
            return climateZone;
        }

        public async Task<ClimateZone?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.ClimateZones
                .Include(z => z.History)
                .Include(z => z.TypicalWeatherPatterns)
                .Include(z => z.Climates).ThenInclude(zd => zd.ClimateDetail)
                .Include(z => z.Seasons).ThenInclude(zs => zs.Season)
                .Include(z => z.Locations).ThenInclude(lz => lz.Location)
                .AsNoTracking()
                .FirstOrDefaultAsync(z => z.Id == id, cancellationToken);
        }

        public async Task<ClimateZone?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.ClimateZones
                .AsNoTracking()
                .FirstOrDefaultAsync(z => z.Id == id, cancellationToken);
        }

        public async Task<List<ClimateZone>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.ClimateZones.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(z => z.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<ClimateZone> UpdateAsync(ClimateZone climateZone, CancellationToken cancellationToken = default)
        {
            _context.Entry(climateZone).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return climateZone;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.ClimateZones
                .Where(z => z.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> IsClimateDetailLinkedAsync(int climateZoneId, int climateDetailId, CancellationToken cancellationToken = default)
        {
            return await _context.ClimateZoneDetails
                .AnyAsync(zd => zd.ClimateZoneId == climateZoneId && zd.ClimateDetailId == climateDetailId, cancellationToken);
        }

        public async Task AddClimateDetailAsync(int climateZoneId, int climateDetailId, CancellationToken cancellationToken = default)
        {
            _context.ClimateZoneDetails.Add(new ClimateZoneDetail { ClimateZoneId = climateZoneId, ClimateDetailId = climateDetailId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveClimateDetailAsync(int climateZoneId, int climateDetailId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.ClimateZoneDetails
                .Where(zd => zd.ClimateZoneId == climateZoneId && zd.ClimateDetailId == climateDetailId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> IsSeasonLinkedAsync(int climateZoneId, int seasonId, CancellationToken cancellationToken = default)
        {
            return await _context.ClimateZoneSeasons
                .AnyAsync(zs => zs.ClimateZoneId == climateZoneId && zs.SeasonId == seasonId, cancellationToken);
        }

        public async Task AddSeasonAsync(int climateZoneId, int seasonId, CancellationToken cancellationToken = default)
        {
            _context.ClimateZoneSeasons.Add(new ClimateZoneSeason { ClimateZoneId = climateZoneId, SeasonId = seasonId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveSeasonAsync(int climateZoneId, int seasonId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.ClimateZoneSeasons
                .Where(zs => zs.ClimateZoneId == climateZoneId && zs.SeasonId == seasonId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }

        public async Task<bool> IsLocationLinkedAsync(int climateZoneId, int locationId, CancellationToken cancellationToken = default)
        {
            return await _context.LocationClimateZones
                .AnyAsync(lz => lz.ClimateZoneId == climateZoneId && lz.LocationId == locationId, cancellationToken);
        }

        public async Task AddLocationAsync(int climateZoneId, int locationId, CancellationToken cancellationToken = default)
        {
            _context.LocationClimateZones.Add(new LocationClimateZone { ClimateZoneId = climateZoneId, LocationId = locationId });
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> RemoveLocationAsync(int climateZoneId, int locationId, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.LocationClimateZones
                .Where(lz => lz.ClimateZoneId == climateZoneId && lz.LocationId == locationId)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
