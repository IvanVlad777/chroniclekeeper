using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public HistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<History> CreateAsync(History history, CancellationToken cancellationToken = default)
        {
            _context.Histories.Add(history);
            await _context.SaveChangesAsync(cancellationToken);
            return history;
        }

        public async Task<History?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Histories
                .Include(h => h.Timelines)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
        }

        public async Task<History?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Histories
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
        }

        public async Task<List<History>> GetAllAsync(int? worldId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Histories.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(h => h.WorldId == wid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<History> UpdateAsync(History history, CancellationToken cancellationToken = default)
        {
            _context.Entry(history).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return history;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.Histories
                .Where(h => h.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
