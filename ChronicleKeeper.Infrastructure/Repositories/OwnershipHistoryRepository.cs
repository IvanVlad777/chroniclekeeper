using ChronicleKeeper.Core.Entities.Characters.Equipment;
using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChronicleKeeper.Infrastructure.Repositories
{
    public class OwnershipHistoryRepository : IOwnershipHistoryRepository
    {
        private readonly ApplicationDbContext _context;

        public OwnershipHistoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OwnershipHistory> CreateAsync(OwnershipHistory ownershipHistory, CancellationToken cancellationToken = default)
        {
            _context.OwnershipHistories.Add(ownershipHistory);
            await _context.SaveChangesAsync(cancellationToken);
            return ownershipHistory;
        }

        public async Task<OwnershipHistory?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.OwnershipHistories
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<List<OwnershipHistory>> GetAllAsync(int? worldId = null, int? itemId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.OwnershipHistories.AsNoTracking();
            if (worldId is int wid)
            {
                query = query.Where(o => o.WorldId == wid);
            }
            if (itemId is int iid)
            {
                query = query.Where(o => o.ItemId == iid);
            }
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<OwnershipHistory> UpdateAsync(OwnershipHistory ownershipHistory, CancellationToken cancellationToken = default)
        {
            _context.Entry(ownershipHistory).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
            return ownershipHistory;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var deleted = await _context.OwnershipHistories
                .Where(o => o.Id == id)
                .ExecuteDeleteAsync(cancellationToken);
            return deleted > 0;
        }
    }
}
