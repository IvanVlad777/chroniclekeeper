using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class CultureRepository : ICultureRepository
    {
        private readonly ApplicationDbContext _context;

        public CultureRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Culture> CreateAsync(Culture culture, CancellationToken cancellationToken = default)
        {
            _context.Cultures.Add(culture);
            await _context.SaveChangesAsync(cancellationToken);
            return culture;
        }

        public async Task<Culture?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Cultures
                .Include(c => c.Language)
                .Include(c => c.Religion)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<Culture?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Cultures
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<List<Culture>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Cultures.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(c => c.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Culture> UpdateAsync(Culture culture, CancellationToken cancellationToken = default)
        {
            _context.Entry(culture).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return culture;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Cultures
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
