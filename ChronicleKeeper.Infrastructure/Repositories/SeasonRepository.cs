using ChronicleKeeper.Core.Entities.Geography.Climate;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class SeasonRepository : ISeasonRepository
    {
        private readonly ApplicationDbContext _context;

        public SeasonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Season> CreateAsync(Season season, CancellationToken cancellationToken = default)
        {
            _context.Seasons.Add(season);
            await _context.SaveChangesAsync(cancellationToken);
            return season;
        }

        public async Task<Season?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Seasons
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<List<Season>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Seasons.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(s => s.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Season> UpdateAsync(Season season, CancellationToken cancellationToken = default)
        {
            _context.Entry(season).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return season;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Seasons
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
